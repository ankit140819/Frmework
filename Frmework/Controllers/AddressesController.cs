using Microsoft.AspNetCore.Mvc;
using Frmework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Frmework.Controllers
{
    public class AddressesController : Controller
    {
        public IActionResult Index(string q, string city, bool? isDefault)
        {
            var list = GetSampleAddresses();

            if (!string.IsNullOrWhiteSpace(q)) list = list.Where(a => (a.FullName ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) || (a.Line1 ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) || (a.Phone ?? "").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(city) && city != "All") list = list.Where(a => string.Equals(a.City, city, StringComparison.OrdinalIgnoreCase)).ToList();
            if (isDefault.HasValue) list = list.Where(a => a.IsDefault == isDefault.Value).ToList();

            return View("~/Views/Addresses/Index.cshtml", list);
        }

        private List<Address> GetSampleAddresses()
        {
            return new List<Address>
            {
                new Address{ Id = "A001", FullName = "Priya Sharma", Phone = "9876543210", Email = "priya@example.com", Line1 = "12 MG Road", Line2 = "Near Central Mall", City = "Mumbai", State = "Maharashtra", Country = "India", Pincode = "400001", IsDefault = true },
                new Address{ Id = "A002", FullName = "Rahul Mehta", Phone = "9123456780", Email = "rahul@example.com", Line1 = "45 Park Street", Line2 = "Floor 2", City = "Delhi", State = "Delhi", Country = "India", Pincode = "110001", IsDefault = false },
                new Address{ Id = "A003", FullName = "Anita Rao", Phone = "9988776655", Email = "anita@example.com", Line1 = "7 Lake View", Line2 = "Apt 3B", City = "Chennai", State = "Tamil Nadu", Country = "India", Pincode = "600001", IsDefault = false }
            };
        }
    }
}
