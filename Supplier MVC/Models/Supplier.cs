namespace Supplier_MVC.Models
{
    public class Supplier
    {
        public long SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Representative { get; set; }
        public string ContactNo { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
    }
}
