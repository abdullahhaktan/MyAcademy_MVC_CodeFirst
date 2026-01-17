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

            // Eksik günleri 0 ile doldur
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
                horizon: 90, // 90 GÜN
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(PolicySaleForecastResult.LowerBounds),
                confidenceUpperBoundColumn: nameof(PolicySaleForecastResult.UpperBounds)
            );

            var model = pipeline.Fit(dataView);

            var engine = model.CreateTimeSeriesEngine<
                PolicySaleTimeSeriesData,
                PolicySaleForecastResult>(mlContext);

            return engine.Predict();
        }



    }
}
