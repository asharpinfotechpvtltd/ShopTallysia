using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class Enquiry
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string email { get; set; }
        public string Number { get; set; }

        public string Address { get; set; }
     

     

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
