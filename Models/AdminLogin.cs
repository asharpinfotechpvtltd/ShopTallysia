using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class AdminLogin
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
