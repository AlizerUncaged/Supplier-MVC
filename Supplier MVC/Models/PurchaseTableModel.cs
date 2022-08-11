using System;

namespace Supplier_MVC.Models
{
    public class PurchaseTableModel
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
        public string Supplier { get; set; }
        public string Status { get; set; }
        public string Product { get; set; }
        public DateTime Added { get; set; }
        public DateTime Received { get; set; }
    }
}