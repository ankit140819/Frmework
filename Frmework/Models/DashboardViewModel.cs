namespace InventoryDashboardMvc.Models;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int TotalStock { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal OnlineSales { get; set; }
    public decimal OfflineSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public int Distributors { get; set; }
    public int Retailers { get; set; }
    public int SalonsSpas { get; set; }
    public int Wholesalers { get; set; }
    public decimal OutstandingAmount { get; set; }
    public List<ProductStock> Products { get; set; } = new();
}

public record ProductStock(
    string Name,
    string Sku,
    decimal Price,
    int Stock,
    int Sold
);
