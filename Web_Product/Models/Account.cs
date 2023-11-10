using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Product.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }  
        public string Password { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;   
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
       
        public virtual Role Role { get; set; }
    }
}
