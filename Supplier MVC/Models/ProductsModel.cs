using System;
using System.ComponentModel.DataAnnotations;

namespace Supplier_MVC.Models
{
    public class ProductsModel
    {
        [Key]
        public int ProductId { get; set; }

        [MaxLength(50)] public string Name { get; set; }

        [MaxLength(200)] public string Description { get; set; }

        public int Qty { get; set; }

        [MaxLength(50)] public string Unit { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateModified { get; set; }
    }
}