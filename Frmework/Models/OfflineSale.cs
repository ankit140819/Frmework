using System;

namespace Frmework.Models
{
    public class OfflineSale
    {
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellerType { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string GSTNumber { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal WholesalePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string DeliveryStatus { get; set; }
        public string ReturnStatus { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Remarks { get; set; }
    }
}
