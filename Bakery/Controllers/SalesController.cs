using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Bakery.Data;
using Bakery.ViewModels;
using Bakery.Models;

namespace Bakery.Controllers
{
    public class SalesController : Controller
    {
        private readonly BakeryDBContext _context;

        public SalesController(BakeryDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Summary(int? year, DateTime? startDate, DateTime? endDate)
        {
            var ordersQuery = _context.Orders.AsQueryable();
            var suppliesQuery = _context.Supplies.AsQueryable();

            // Примените фильтр по году, если он указан
            if (year.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == year.Value);
                suppliesQuery = suppliesQuery.Where(s => s.SupplyDate.HasValue && s.SupplyDate.Value.Year == year.Value);
            }

            // Примените фильтр по дате заказа, если он указан
            if (startDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value);
                suppliesQuery = suppliesQuery.Where(s => s.SupplyDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate <= endDate.Value);
                suppliesQuery = suppliesQuery.Where(s => s.SupplyDate <= endDate.Value);
            }

            decimal totalRevenue = ordersQuery.Sum(o => o.Price) ?? 0;
            decimal totalSpending = suppliesQuery.Sum(s => s.Price) ?? 0;

            var summaryViewModel = new SummaryViewModel
            {
                TotalRevenue = totalRevenue,
                TotalSpending = totalSpending
            };

            ViewData["Year"] = year;
            ViewData["StartDate"] = startDate?.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate?.ToString("yyyy-MM-dd");

            return View("Summary", summaryViewModel);
        }

        public IActionResult TotalRevenueButton(int? year, DateTime? startDate, DateTime? endDate)
        {
            var totalRevenue = CalculateTotalRevenue(year, startDate, endDate);
            return Content(totalRevenue.ToString("C"));
        }

        public IActionResult TotalSpendingButton(int? year, DateTime? startDate, DateTime? endDate)
        {
            var totalSpending = CalculateTotalSpending(year, startDate, endDate);
            return Content(totalSpending.ToString("C"));
        }

        private decimal CalculateTotalRevenue(int? year, DateTime? startDate, DateTime? endDate)
        {
            var ordersQuery = _context.Orders.AsQueryable();

            if (year.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == year.Value);
            }

            if (startDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate <= endDate.Value.Date);
            }

            return ordersQuery.Sum(o => o.Price) ?? 0;
        }

        private decimal CalculateTotalSpending(int? year, DateTime? startDate, DateTime? endDate)
        {
            var suppliesQuery = _context.Supplies.AsQueryable();

            if (year.HasValue)
            {
                suppliesQuery = suppliesQuery.Where(s => s.SupplyDate.HasValue && s.SupplyDate.Value.Year == year.Value);
            }

            if (startDate.HasValue)
            {
                suppliesQuery = suppliesQuery.Where(s => s.SupplyDate >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                suppliesQuery = suppliesQuery.Where(s => s.SupplyDate <= endDate.Value.Date);
            }

            return suppliesQuery.Sum(s => s.Price) ?? 0;
        }
    }
}
