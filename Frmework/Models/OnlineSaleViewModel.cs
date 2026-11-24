using System;
using System.Collections.Generic;

namespace Frmework.Models
{
    public class OnlineSaleViewModel
    {
        public List<OnlineSale> Sales { get; set; } = new List<OnlineSale>();

        // Summary
        public decimal TotalSalesAmount { get; set; }
        public int TotalOrders { get; set; }
        public int TotalQuantity { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public int ReturnsCount { get; set; }

        // Platform breakdown
        public decimal AmazonSales { get; set; }
        public decimal FlipkartSales { get; set; }
        public decimal MeeshoSales { get; set; }
        public decimal WebsiteSales { get; set; }
    }
}
