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
        private List<PolicySaleTimeSeriesData> GetDailyPolicySales(
            List<ResultPolicySaleDto> sales,
            int dayCount)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-dayCount);

            var dailyData = sales
                .Where(s => s.StartDate >= startDate && s.StartDate < endDate)
                .GroupBy(s => s.StartDate.Date)
                .Select(g => new PolicySaleTimeSeriesData
                {
                    Date = g.Key,
                    SaleCount = g.Count()
                })
                .ToList();

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

            return dailyData.OrderBy(d => d.Date).ToList();
        }

        public PolicySaleForecastResult ForecastNext3Months(
            List<ResultPolicySaleDto> sales,
            int dayCount = 120)
        {
            var dailyData = GetDailyPolicySales(sales, dayCount);

            if (dailyData.Count < 30)
                return null;

            var mlContext = new MLContext(seed: 42);
            int windowSize = Math.Min(30, dailyData.Count / 2);
            var dataView = mlContext.Data.LoadFromEnumerable(dailyData);

            var pipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(PolicySaleForecastResult.ForecastedCounts),
                inputColumnName: nameof(PolicySaleTimeSeriesData.SaleCount),
                windowSize: windowSize,
                seriesLength: dailyData.Count,
                trainSize: dailyData.Count,
                horizon: 90,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(PolicySaleForecastResult.LowerBounds),
                confidenceUpperBoundColumn: nameof(PolicySaleForecastResult.UpperBounds)
            );

            var model = pipeline.Fit(dataView);
            var engine = model.CreateTimeSeriesEngine<PolicySaleTimeSeriesData, PolicySaleForecastResult>(mlContext);
            // ... engine.Predict() satırından sonra
            var prediction = engine.Predict();

            // Negatif değerleri sıfıra yuvarla (Clamping)
            for (int i = 0; i < prediction.ForecastedCounts.Length; i++)
            {
                if (prediction.ForecastedCounts[i] < 0) prediction.ForecastedCounts[i] = 0;
                if (prediction.LowerBounds[i] < 0) prediction.LowerBounds[i] = 0;
                if (prediction.UpperBounds[i] < 0) prediction.UpperBounds[i] = 0;
            }

            return prediction;
        }

        public Dictionary<string, PolicySaleForecastResult> ForecastNext3MonthsByCity(
            List<ResultPolicySaleDto> sales,
            int dayCount = 120)
        {
            // Customer null olmayanları filtrele
            var validSales = sales.Where(s => s.Customer != null && !string.IsNullOrEmpty(s.City)).ToList();

            var cities = validSales.Select(s => s.City).Distinct();
            var result = new Dictionary<string, PolicySaleForecastResult>();

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
