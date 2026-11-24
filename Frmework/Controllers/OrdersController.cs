using Microsoft.AspNetCore.Mvc;
using Frmework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frmework.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index(string q, string source, string orderStatus, string paymentStatus, DateTime? from, DateTime? to)
        {
            var orders = GetSampleOrders();

            if (!string.IsNullOrWhiteSpace(q)) orders = orders.Where(o => (o.OrderId ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) || (o.ProductName ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(source) && source != "All") orders = orders.Where(o => o.Source == source).ToList();
            if (!string.IsNullOrWhiteSpace(orderStatus) && orderStatus != "All") orders = orders.Where(o => o.OrderStatus == orderStatus).ToList();
            if (!string.IsNullOrWhiteSpace(paymentStatus) && paymentStatus != "All") orders = orders.Where(o => o.PaymentStatus == paymentStatus).ToList();
            if (from.HasValue) orders = orders.Where(o => o.OrderDate >= from.Value.Date).ToList();
            if (to.HasValue) orders = orders.Where(o => o.OrderDate <= to.Value.Date).ToList();

            return View("~/Views/Orders/Index.cshtml", orders);
        }

        private List<OrderViewModel> GetSampleOrders()
        {
            var now = DateTime.Now;
            return new List<OrderViewModel>
            {
                new OrderViewModel{ OrderId = "AMZ1001", Source = "Amazon", Type = "Online", OrderDate = now.AddDays(-3), ProductName = "Collagen Booster Cream", SKU = "CB001", Quantity = 2, MRP = 1499, Price = 1299, Discount = 200, PlatformCharges = 50, ShippingCharges = 40, GST = 45, NetAmount = 1299*2 -200, PaymentStatus = "Paid", OrderStatus = "Delivered", ReturnStatus = "None", SettlementAmount = 2390, Remarks = ""},
                new OrderViewModel{ OrderId = "FLP2002", Source = "Flipkart", Type = "Online", OrderDate = now.AddDays(-7), ProductName = "Hyaluronic Acid Face Serum", SKU = "HA002", Quantity = 1, MRP = 959, Price = 899, Discount = 60, PlatformCharges = 30, ShippingCharges = 20, GST = 18, NetAmount = 899 - 60, PaymentStatus = "Pending", OrderStatus = "Processing", ReturnStatus = "None", SettlementAmount = 0, Remarks = ""},
                new OrderViewModel{ OrderId = "OFF3003", Source = "Offline", Type = "Offline", OrderDate = now.AddDays(-10), ProductName = "Niacinamide Face Serum", SKU = "NI003", Quantity = 10, MRP = 899, Price = 700, Discount = 50, PlatformCharges = 0, ShippingCharges = 0, GST = 18, NetAmount = 700*10, PaymentStatus = "Paid", OrderStatus = "Delivered", ReturnStatus = "None", SettlementAmount = 7000, Remarks = "Wholesale"}
            };
        }
    }
}
