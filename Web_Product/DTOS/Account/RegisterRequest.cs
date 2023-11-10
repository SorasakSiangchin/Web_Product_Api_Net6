using System.ComponentModel.DataAnnotations;

namespace Web_Product.DTOS.Account
{
    // เอาไว้สมัคร
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        // ขั้นต่ำ 8 ตัว
        [MinLength(8)]
        public string Password { get; set; }
        public int RoleId { get; set; }

    }
}
