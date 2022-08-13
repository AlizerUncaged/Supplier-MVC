namespace Supplier_MVC.Models
{
    public class BuyModel
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }

        public string Address { get; set; }

        public string PaymentType { get; set; }
    }
}