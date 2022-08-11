using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supplier_MVC.Models
{
    public class PurchaseOrderDetailsModel
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [TableHidden]
        public int PurchaseOrderHeaderId { get; set; }
        
        [TableHidden]
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }


        [ForeignKey("PurchaseOrderHeaderId")]
        public virtual PurchaseOrderHeadersModel PurchaseOrderHeaders { get; set; }
    }
}