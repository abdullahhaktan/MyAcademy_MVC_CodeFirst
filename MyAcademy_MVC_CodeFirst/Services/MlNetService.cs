using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using MyAcademy_MVC_CodeFirst.DTOs.PolicySaleDtos;
using MyAcademy_MVC_CodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAcademy_MVC_CodeFirst.Services
{
    public class MlNetService
    {
        // Prepare daily time series data from policy sales
        private List<PolicySaleTimeSeriesData> GetDailyPolicySales(
            List<ResultPolicySaleDto> sales,
            int dayCount)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-dayCount); // Go back specified number of days

            // Group sales by date and count daily sales
            var dailyData = sales
                .Where(s => s.StartDate >= startDate && s.StartDate < endDate)
                .GroupBy(s => s.StartDate.Date)
                .Select(g => new PolicySaleTimeSeriesData
                {
                    Date = g.Key,
                    SaleCount = g.Count()
                })
                .ToList();

            // Fill missing days with zero sales (ensure continuous time series)
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                if (!dailyData.Any(d => d.Date == date))
                {
                    dailyData.Add(new PolicySaleTimeSeriesData
                    {
                        Date = date,
                        SaleCount = 0
                    });
                }
            }

            return dailyData.OrderBy(d => d.Date).ToList(); // Return sorted by date
        }

        // Forecast policy sales for next 3 months using ML.NET time series
        public PolicySaleForecastResult ForecastNext3Months(
            List<ResultPolicySaleDto> sales,
            int dayCount = 120) // Default: Use last 120 days of data
        {
            var dailyData = GetDailyPolicySales(sales, dayCount);

            // Need minimum data for meaningful forecast
            if (dailyData.Count < 30)
                return null;

            // Initialize ML.NET context with seed for reproducibility
            var mlContext = new MLContext(seed: 42);

            // Calculate window size for SSA algorithm
            int windowSize = Math.Min(30, dailyData.Count / 2);
            var dataView = mlContext.Data.LoadFromEnumerable(dailyData);

            // Configure time series forecasting pipeline using SSA (Singular Spectrum Analysis)
            var pipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(PolicySaleForecastResult.ForecastedCounts),
                inputColumnName: nameof(PolicySaleTimeSeriesData.SaleCount),
                windowSize: windowSize, // Moving window size
                seriesLength: dailyData.Count, // Total data points
                trainSize: dailyData.Count, // Use all data for training
                horizon: 90, // Forecast 90 days (3 months)
                confidenceLevel: 0.95f, // 95% confidence interval
                confidenceLowerBoundColumn: nameof(PolicySaleForecastResult.LowerBounds),
                confidenceUpperBoundColumn: nameof(PolicySaleForecastResult.UpperBounds)
            );

            // Train model and create prediction engine
            var model = pipeline.Fit(dataView);
            var engine = model.CreateTimeSeriesEngine<PolicySaleTimeSeriesData, PolicySaleForecastResult>(mlContext);

            // Generate prediction
            var prediction = engine.Predict();

            // Clamp negative values to zero (sales can't be negative)
            for (int i = 0; i < prediction.ForecastedCounts.Length; i++)
            {
                if (prediction.ForecastedCounts[i] < 0) prediction.ForecastedCounts[i] = 0;
                if (prediction.LowerBounds[i] < 0) prediction.LowerBounds[i] = 0;
                if (prediction.UpperBounds[i] < 0) prediction.UpperBounds[i] = 0;
            }

            return prediction;
        }

        // Forecast sales for each city separately
        public Dictionary<string, PolicySaleForecastResult> ForecastNext3MonthsByCity(
            List<ResultPolicySaleDto> sales,
            int dayCount = 120)
        {
            // Filter out invalid records (null customer or empty city)
            var validSales = sales.Where(s => s.Customer != null && !string.IsNullOrEmpty(s.City)).ToList();

            var cities = validSales.Select(s => s.City).Distinct();
            var result = new Dictionary<string, PolicySaleForecastResult>();

            // Forecast for each city individually
            foreach (var city in cities)
            {
                var citySales = validSales.Where(s => s.City == city).ToList();
                var forecast = ForecastNext3Months(citySales, dayCount);
                if (forecast != null)
                    result[city] = forecast;
            }

            return result;
        }
    }
}