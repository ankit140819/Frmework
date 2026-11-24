using System.Collections.Generic;

namespace Frmework.Models
{
    public class OnlineSalesViewModel
    {
        public List<ProductStockViewModel> Products { get; set; } = new List<ProductStockViewModel>();
        public List<PlatformSalesViewModel> Platforms { get; set; } = new List<PlatformSalesViewModel>();
        public List<OnlineSale> Sales { get; set; } = new List<OnlineSale>();

        // Summary
        public int TotalProducts => Products?.Count ?? 0;
        public int TotalStock { get; set; }
        public int TotalUnitsSold { get; set; }
        public int TotalRemainingStock { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public int TotalOnlineOrders { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }

        // Backwards-compatible properties used by the view
        public int TotalOrders { get; set; }
        public int TotalQuantity { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public int ReturnsCount { get; set; }
        public decimal AmazonSales { get; set; }
        public decimal FlipkartSales { get; set; }
        public decimal MeeshoSales { get; set; }
        public decimal WebsiteSales { get; set; }
    }
}
