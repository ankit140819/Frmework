using System;

namespace Frmework.Models
{
    public class ProductComment
    {
        public string ProductId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
