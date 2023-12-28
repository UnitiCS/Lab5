using System;
using System.ComponentModel.DataAnnotations;

namespace Bakery.ViewModels
{
    public class SummaryViewModel
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalSpending { get; set; }
    }
}
