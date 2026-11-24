using Microsoft.AspNetCore.Mvc;
using Frmework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frmework.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index(string q, string city, bool? active)
        {
            var list = GetSampleCustomers();

            if (!string.IsNullOrWhiteSpace(q)) list = list.Where(c => (c.FullName ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) || (c.Email ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) || (c.Mobile ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(city) && city != "All") list = list.Where(c => c.City == city).ToList();
            if (active.HasValue) list = list.Where(c => c.IsActive == active.Value).ToList();

            return View("~/Views/Customers/Index.cshtml", list);
        }

        private List<Customer> GetSampleCustomers()
        {
            var now = DateTime.Now;
            return new List<Customer>
            {
                new Customer{ CustomerId = "C001", FullName = "Priya Sharma", Email = "priya@example.com", Mobile = "9876543210", City = "Mumbai", TotalOrders = 5, TotalSpent = 7495, LastOrderDate = now.AddDays(-10), IsActive = true },
                new Customer{ CustomerId = "C002", FullName = "Rahul Mehta", Email = "rahul@example.com", Mobile = "9123456780", City = "Delhi", TotalOrders = 2, TotalSpent = 1798, LastOrderDate = now.AddDays(-20), IsActive = true },
                new Customer{ CustomerId = "C003", FullName = "Anita Rao", Email = "anita@example.com", Mobile = "9988776655", City = "Chennai", TotalOrders = 1, TotalSpent = 999, LastOrderDate = now.AddDays(-2), IsActive = true }
            };
        }
    }
}
