using System;

namespace Frmework.Models
{
    public class OnlineSale
    {
        public string SellerPlatform { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal MRP { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal PlatformCharges { get; set; }
        public decimal ShippingCharges { get; set; }
        public decimal GST { get; set; }
        public decimal NetSaleAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string ReturnStatus { get; set; }
        public decimal SettlementAmount { get; set; }
        public DateTime? SettlementDate { get; set; }
        public string Remarks { get; set; }
    }
}
