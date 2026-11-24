using System;
using System.Collections.Generic;

namespace Frmework.Models
{
    public class OfflineSaleViewModel
    {
        public List<OfflineSale> Sales { get; set; } = new List<OfflineSale>();

        // Summary
        public decimal TotalSalesAmount { get; set; }
        public int TotalOrders { get; set; }
        public int TotalQuantity { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public int ReturnsCount { get; set; }

        // Offline breakdown
        public decimal WholesalerSales { get; set; }
        public decimal DistributorSales { get; set; }
        public decimal RetailerSales { get; set; }
        public decimal SalonSales { get; set; }
    }
}
