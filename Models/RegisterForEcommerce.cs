using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class RegisterForEcommerce
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
      
        public string Mobile { get; set; }
        public string Email { get; set; }
   
        public string Password { get; set; }
        public string RegistrationNumber { get; set; }
    }
}
