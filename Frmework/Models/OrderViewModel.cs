using System;

namespace Frmework.Models
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public string Source { get; set; } // Amazon, Flipkart, Offline Seller
        public string Type { get; set; } // Online / Offline
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PlatformCharges { get; set; }
        public decimal ShippingCharges { get; set; }
        public decimal GST { get; set; }
        public decimal NetAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string ReturnStatus { get; set; }
        public decimal SettlementAmount { get; set; }
        public string Remarks { get; set; }
    }
}
