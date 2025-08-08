using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ProfilePhotoPath { get; set; }

        public string IdProofPath { get; set; }
        public string UserType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
