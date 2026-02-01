using MyAcademy_MVC_CodeFirst.Data.Context;
using MyAcademy_MVC_CodeFirst.DTOs.PolicySaleDtos;
using MyAcademy_MVC_CodeFirst.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAcademy_MVC_CodeFirst.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // Database context and ML service for forecasting
        private readonly AppDbContext context = new AppDbContext();
        private readonly MlNetService mlNetService = new MlNetService();

        public async Task<ActionResult> Index()
        {
            // Load policy sales data with related customer information
            var values = await context.PolicySales.Include(ps => ps.Customer).ToListAsync();
            var policySales = MvcApplication.mapperInstance
                .Map<List<ResultPolicySaleDto>>(values);

            // Generate 3-month sales forecast using ML.NET
            var forecast = mlNetService.ForecastNext3Months(policySales, 100);

            if (forecast == null)
                return View(); // Return empty view if forecast fails

            // --- AGGREGATE DAILY FORECASTS INTO MONTHLY TOTALS ---
            var monthly = new Dictionary<string, (float sum, float low, float up)>();
            for (int i = 0; i < forecast.ForecastedCounts.Length; i++)
            {
                var date = DateTime.Now.Date.AddDays(i + 1);
                var key = date.ToString("yyyy-MM"); // Month key format
                if (!monthly.ContainsKey(key))
                    monthly[key] = (0, 0, 0);

                // Accumulate daily forecasts into monthly totals
                var current = monthly[key];
                monthly[key] = (
                    current.sum + forecast.ForecastedCounts[i],
                    current.low + forecast.LowerBounds[i],
                    current.up + forecast.UpperBounds[i]
                );
            }

            // Get first 3 months of forecast
            var first3Months = monthly.OrderBy(x => x.Key).Take(3).ToList();
            ViewBag.monthLabels = first3Months.Select(x => x.Key).ToList();
            ViewBag.lineChartDatas = first3Months.Select(x => Math.Round(x.Value.sum)).ToList();
            ViewBag.lowerBound = first3Months
                .Select(x => Math.Round(x.Value.low))
                .Select(v => v < 0 ? 0 : v) // Clamp negative values to zero
                .ToList();
            ViewBag.upperBound = first3Months.Select(x => Math.Round(x.Value.up)).ToList();

            // --- CUSTOMER TREND ANALYSIS ---
            var totalCustomerCountNow = await context.Customers.CountAsync();
            var sixMonthsAgo = DateTime.Now.AddMonths(-6);

            // Count customers registered before 6 months ago
            var totalCustomerCountBefore = await context.Customers
                .Where(c => c.RegisteredAt < sixMonthsAgo)
                .CountAsync();

            // Calculate percentage change in customer count
            double percentageChange = 0;
            if (totalCustomerCountBefore > 0)
                percentageChange = ((double)(totalCustomerCountNow - totalCustomerCountBefore) / totalCustomerCountBefore) * 100;
            else if (totalCustomerCountNow > 0)
                percentageChange = 100; // 100% growth if starting from zero

            ViewBag.CustomerTrend = percentageChange.ToString("F1"); // Format to 1 decimal place
            ViewBag.totalCustomerCount = totalCustomerCountNow.ToString("N0"); // Format with thousand separators
            ViewBag.activePolicyCount = (await context.Policies.Where(p => p.IsActive).CountAsync()).ToString("N0");
            ViewBag.unansweredMessageCount = (await context.ContactMessages.Where(m => m.IsReplied == false).CountAsync()).ToString("N0");

            // --- LAST MONTH POLICY SALE REVENUE ---
            var lastMonth = DateTime.Now.AddMonths(-1);
            var lastMonthPolicySaleSum = await context.PolicySales
                .Where(ps => ps.StartDate >= lastMonth)
                .SumAsync(ps => ps.Amount);

            ViewBag.lastMonthPolicySaleSum = lastMonthPolicySaleSum.ToString("N2"); // Currency format

            // --- POLICY TYPE DISTRIBUTION ANALYSIS ---
            var totalPolicySaleCount = await context.PolicySales.CountAsync();

            // Car insurance percentage
            var totalCarInsuranceSaleCount = await context.PolicySales
                .Where(ps => ps.PolicyId == 7 || ps.PolicyId == 12) // Specific policy IDs for car insurance
                .CountAsync();

            ViewBag.totalCarInsuranceSaleCountPercentage =
                totalPolicySaleCount > 0
                ? (((double)totalCarInsuranceSaleCount / totalPolicySaleCount) * 100).ToString("F1")
                : "0.0";

            // Life insurance percentage
            var totalLifeInsuranceSaleCount = await context.PolicySales.Where(ps => ps.PolicyId == 4).CountAsync();
            ViewBag.totalLifeInsuranceSalePercentage =
                totalPolicySaleCount > 0
                ? (((double)totalLifeInsuranceSaleCount / totalPolicySaleCount) * 100).ToString("F1")
                : "0.0";

            // Residential insurance percentage
            var totalResidentalCount = await context.PolicySales
                .Where(ps => ps.PolicyId == 19 || ps.PolicyId == 22)
                .CountAsync();

            ViewBag.totalResidentalCountPercentage =
                totalPolicySaleCount > 0
                ? (((double)totalResidentalCount / totalPolicySaleCount) * 100).ToString("F1")
                : "0.0";

            // Health insurance percentage
            var totalHealthInsuranceSaleCount = await context.PolicySales
                .Where(ps => ps.PolicyId == 6 || ps.PolicyId == 18)
                .CountAsync();

            ViewBag.totalHealthInsuranceSaleCountPercentage =
                totalPolicySaleCount > 0
                ? (((double)totalHealthInsuranceSaleCount / totalPolicySaleCount) * 100).ToString("F1")
                : "0.0";

            // --- LAST 6 MONTHS DAILY SALES AGGREGATED INTO 30-DAY BUCKETS ---
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-180); // 6 months = ~180 days

            var results = await context.PolicySales
                .Where(ps => ps.StartDate >= startDate && ps.StartDate <= endDate)
                .GroupBy(ps => DbFunctions.TruncateTime(ps.StartDate)) // Group by date (ignore time)
                .Select(g => new { Date = g.Key.Value, Count = g.Count() })
                .ToListAsync();

            // Create 6 buckets for 6 months (30 days each)
            var dailyCounts = new Dictionary<int, int> { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

            for (int i = 0; i < 180; i++)
            {
                var day = startDate.AddDays(i);
                var item = results.FirstOrDefault(x => x.Date == day);
                var count1 = item != null ? item.Count : 0;

                // Distribute counts into 30-day buckets
                if (i < 30) dailyCounts[0] += count1;
                else if (i < 60) dailyCounts[1] += count1;
                else if (i < 90) dailyCounts[2] += count1;
                else if (i < 120) dailyCounts[3] += count1;
                else if (i < 150) dailyCounts[4] += count1;
                else dailyCounts[5] += count1;
            }

            ViewBag.dailyCounts = dailyCounts.Values.ToList();
            ViewBag.last6MonthLabels = new List<string>
            {
                DateTime.Now.AddMonths(-5).ToString("MMM"), // Abbreviated month names
                DateTime.Now.AddMonths(-4).ToString("MMM"),
                DateTime.Now.AddMonths(-3).ToString("MMM"),
                DateTime.Now.AddMonths(-2).ToString("MMM"),
                DateTime.Now.AddMonths(-1).ToString("MMM"),
                DateTime.Now.ToString("MMM")
            };

            // --- LAST 5 POLICY SALES FOR RECENT ACTIVITY ---
            var policySaleValues = await context.PolicySales
                .OrderByDescending(ps => ps.Id) // Get most recent sales
                .Take(5)
                .Include(ps => ps.Customer)
                .Include(ps => ps.Policy)
                .ToListAsync();

            var lastFivePolicySales = MvcApplication.mapperInstance.Map<List<ResultPolicySaleDto>>(policySaleValues);

            // --- CITY-BASED 3-MONTH FORECASTS ---
            var cityForecasts = mlNetService.ForecastNext3MonthsByCity(policySales, 100);

            var cityLabels = cityForecasts.Keys.ToList();
            var cityDatas = new Dictionary<string, List<double>>();
            var next3Months = new List<string>();

            // Process forecast for each city
            foreach (var city in cityLabels)
            {
                var forecastCity = cityForecasts[city];
                var monthlyCity = new Dictionary<string, double>();

                // Aggregate daily forecasts into monthly totals for this city
                for (int i = 0; i < forecastCity.ForecastedCounts.Length; i++)
                {
                    var date = DateTime.Now.Date.AddDays(i + 1);
                    var key = date.ToString("yyyy-MM");
                    if (!monthlyCity.ContainsKey(key))
                        monthlyCity[key] = 0;
                    monthlyCity[key] += forecastCity.ForecastedCounts[i];
                }

                // Get first 3 months of forecast for this city
                var first3 = monthlyCity.OrderBy(x => x.Key).Take(3).Select(x => Math.Round(x.Value)).ToList();

                cityDatas[city] = first3;

                // Collect month labels (only once)
                if (next3Months.Count == 0)
                    next3Months = monthlyCity.OrderBy(x => x.Key).Take(3).Select(x => x.Key).ToList();
            }

            ViewBag.CityLabels = cityLabels;
            ViewBag.CityForecasts = cityDatas;
            ViewBag.Next3Months = next3Months;

            return View(lastFivePolicySales);
        }
    }
}