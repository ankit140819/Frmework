using System;

namespace Frmework.Models
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public bool IsActive { get; set; }
    }
}
