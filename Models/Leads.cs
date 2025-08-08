using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class Leads
    {
        [Key]
        public int LeadId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Message { get; set; }

        public string LeadSource { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
