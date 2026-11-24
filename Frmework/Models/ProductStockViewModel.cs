using System.Collections.Generic;

namespace Frmework.Models
{
    public class ProductStockViewModel
    {
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public decimal MRP { get; set; }
        public int OpeningStock { get; set; }
        public int TotalAdded { get; set; }
        public int TotalSold { get; set; }
        public int OfflineSold { get; set; }
        public int RemainingStock { get; set; }
        public decimal SalesAmount { get; set; }
        public string StockStatus { get; set; }
        public int RemainingPercent { get; set; }
        // units sold per platform
        public Dictionary<string,int> PlatformSold { get; set; } = new Dictionary<string,int>();
    }
}
