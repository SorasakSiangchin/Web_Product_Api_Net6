using System.ComponentModel.DataAnnotations;

namespace Web_Product.DTOS.Account
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

    }
}
