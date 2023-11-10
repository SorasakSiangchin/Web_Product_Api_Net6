using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Product.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }   
        public DateTime Created { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever] 
        //internal Category Category { get; set; }
        // internal ใช้ในเฉพาะในโปรเจ็คขา้งนอกไม่เห็น
        public Category Category { get; set; }
        //[NotMapped]
        //public IFormFileCollection? FormFiles { get; set; }
    }
}
