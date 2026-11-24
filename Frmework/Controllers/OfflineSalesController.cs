using Microsoft.AspNetCore.Mvc;
using Frmework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frmework.Controllers
{
    public class OfflineSalesController : Controller
    {
        public IActionResult Index(string q, string sellerType, string paymentStatus, DateTime? from, DateTime? to)
        {
            var sample = GetSampleOfflineSales();

            if (!string.IsNullOrWhiteSpace(q))
            {
                sample = sample.Where(s => (s.SellerName ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)
                                            || (s.ContactPerson ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)
                                            || (s.SellerId ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(sellerType) && sellerType != "All")
            {
                sample = sample.Where(s => string.Equals(s.SellerType, sellerType, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(paymentStatus) && paymentStatus != "All")
            {
                sample = sample.Where(s => string.Equals(s.PaymentStatus, paymentStatus, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (from.HasValue)
                sample = sample.Where(s => s.OrderDate >= from.Value.Date).ToList();
            if (to.HasValue)
                sample = sample.Where(s => s.OrderDate <= to.Value.Date).ToList();

            var vm = new OfflineSaleViewModel();
            vm.Sales = sample;
            vm.TotalOrders = vm.Sales.Count;
            vm.TotalQuantity = vm.Sales.Sum(s => s.Quantity);
            vm.TotalSalesAmount = vm.Sales.Sum(s => s.TotalAmount);
            vm.PaidAmount = vm.Sales.Where(s => s.PaymentStatus == "Paid").Sum(s => s.TotalAmount);
            vm.PendingAmount = vm.Sales.Where(s => s.PaymentStatus == "Pending").Sum(s => s.TotalAmount);
            vm.ReturnsCount = vm.Sales.Count(s => s.ReturnStatus == "Returned" || s.ReturnStatus == "Refunded");

            vm.WholesalerSales = vm.Sales.Where(s => s.SellerType == "Wholesaler").Sum(s => s.TotalAmount);
            vm.DistributorSales = vm.Sales.Where(s => s.SellerType == "Distributor").Sum(s => s.TotalAmount);
            vm.RetailerSales = vm.Sales.Where(s => s.SellerType == "Retailer").Sum(s => s.TotalAmount);
            vm.SalonSales = vm.Sales.Where(s => s.SellerType == "Salon").Sum(s => s.TotalAmount);

            return View("~/Views/OfflineSales/Index.cshtml", vm);
        }

        private List<OfflineSale> GetSampleOfflineSales()
        {
            var now = DateTime.Now;
            var list = new List<OfflineSale>
            {
                new OfflineSale{ SellerId = "WH001", SellerName = "Apex Wholesalers", SellerType = "Wholesaler", ContactPerson = "Mr. Kumar", MobileNumber = "9876543210", Email = "apex@example.com", Address = "Mumbai", GSTNumber = "27ABCDE1234F1Z5", ProductName = "Collagen Booster Cream", SKU = "CB001", Quantity = 50, MRP = 1499, WholesalePrice = 900, SellingPrice = 950, Discount = 50, TotalAmount = 950*50, PaymentMode = "Bank Transfer", PaymentStatus = "Paid", OrderDate = now.AddDays(-12), DeliveryStatus = "Delivered", ReturnStatus = "None", OutstandingAmount = 0, Remarks = "Monthly stock"},
                new OfflineSale{ SellerId = "DS002", SellerName = "Distributor One", SellerType = "Distributor", ContactPerson = "Rita Singh", MobileNumber = "9123456780", Email = "dist1@example.com", Address = "Delhi", GSTNumber = "07ABCDE1234F1Z6", ProductName = "Hyaluronic Serum", SKU = "HA002", Quantity = 20, MRP = 959, WholesalePrice = 600, SellingPrice = 650, Discount = 50, TotalAmount = 650*20, PaymentMode = "Cash", PaymentStatus = "Pending", OrderDate = now.AddDays(-4), DeliveryStatus = "Processing", ReturnStatus = "None", OutstandingAmount = 5000, Remarks = "Payment due"},
                new OfflineSale{ SellerId = "RT003", SellerName = "Retailer Store", SellerType = "Retailer", ContactPerson = "Sunita", MobileNumber = "9988776655", Email = "retail@example.com", Address = "Chennai", GSTNumber = "33ABCDE1234F1Z7", ProductName = "Vitamin C Serum", SKU = "VC005", Quantity = 10, MRP = 999, WholesalePrice = 550, SellingPrice = 700, Discount = 50, TotalAmount = 700*10, PaymentMode = "UPI", PaymentStatus = "Paid", OrderDate = now.AddDays(-2), DeliveryStatus = "Delivered", ReturnStatus = "None", OutstandingAmount = 0, Remarks = "Regular order"}
            };

            return list;
        }
    }
}
