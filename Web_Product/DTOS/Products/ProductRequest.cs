using System.ComponentModel.DataAnnotations;

namespace Web_Product.DTOS.Product
{
    public class ProductRequest
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "no more than {1} chars")]
        public string Name { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "between {1}-{2}")]
        public int Stock { get; set; }

        [Required]
        [Range(0, 1_000_000, ErrorMessage = "between {1}-{2}")]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFileCollection? FormFiles { get; set; }

    }
}
