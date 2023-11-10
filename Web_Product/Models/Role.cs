using System.ComponentModel.DataAnnotations;

namespace Web_Product.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}
