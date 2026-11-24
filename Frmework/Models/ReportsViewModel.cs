using System.Collections.Generic;

namespace Frmework.Models
{
    public class TopProductItem
    {
        public string ProductName { get; set; }
        public int UnitsSold { get; set; }
        public decimal SalesAmount { get; set; }
    }

    public class PlatformReportItem
    {
        public string Platform { get; set; }
        public int Orders { get; set; }
        public int UnitsSold { get; set; }
        public decimal SalesAmount { get; set; }
    }

    public class ReportsViewModel
    {
        public decimal TotalSalesAmount { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUnitsSold { get; set; }
        public int TotalCustomers { get; set; }
        public List<TopProductItem> TopProducts { get; set; } = new List<TopProductItem>();
        public List<PlatformReportItem> PlatformSummaries { get; set; } = new List<PlatformReportItem>();
    }
}
