using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class FaultRepotingForUsers
    {
        [Key]
        public int TicketId { get; set; }
     
        public string Name { get; set; }
        public string email { get; set; }
        public string Number { get; set; }

        public string Address { get; set; }
        public string Description { get; set; }

        public string Status { get; set; } = "Open";

        public DateTime CreatedAt { get; set; } = DateTime.Now;




     
    }
}
