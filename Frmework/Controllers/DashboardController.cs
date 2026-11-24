using InventoryDashboardMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Frmework.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalProducts = 5,
                TotalStock = 1240,
                LowStockProducts = 2,
                OutOfStockProducts = 0,
                OnlineSales = 186500,
                OfflineSales = 243800,
                TotalRevenue = 430300,
                Distributors = 12,
                Retailers = 48,
                SalonsSpas = 27,
                Wholesalers = 9,
                OutstandingAmount = 78400,
                Products = new List<ProductStock>
                {
                    new("Collagen Booster Cream", "CB001", 1499, 320, 45),
                    new("Hyaluronic Acid Face Serum", "HA002", 959, 260, 30),
                    new("Niacinamide Face Serum", "NI003", 899, 210, 25),
                    new("Lustrous Gel Cream", "LG004", 1199, 190, 20),
                    new("Vitamin C Face Serum", "VC005", 999, 260, 35)
                }
            };

            return View("~/Views/Dashboard/Index.cshtml", model);
        }

        // Serve the ProductInventory view
        public IActionResult ProductInventory()
        {
            var model = new DashboardInventoryModel
            {
                TotalStock = 0 // default, replace with real value if available
            };

            return View("~/Views/Dashboard/ProductInventory.cshtml", model);
        }
        public IActionResult OrdersInventory()
        {
            var model = new DashboardInventoryModel
            {
                TotalStock = 0
            };
            return View("~/Views/Dashboard/OrdersInventory.cshtml", model);
        }

        public IActionResult CustomerInventory()
        {
            var model = new DashboardInventoryModel
            {
                TotalStock = 0
            };
            return View("~/Views/Dashboard/CustomerInventory.cshtml", model);
        }
    }
}
