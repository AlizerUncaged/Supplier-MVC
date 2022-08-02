using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supplier_MVC.Models
{
    public class PurchaseOrderHeadersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int SupplierId { get; set; }

        [MaxLength(50)] public string Status { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateReceived { get; set; }
    }
}