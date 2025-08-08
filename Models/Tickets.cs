using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
  
        public class Ticket
        {
            [Key]
            public int TicketId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

            public string Subject { get; set; }

            public string Description { get; set; }

            public string Status { get; set; } = "Open";

            public DateTime CreatedAt { get; set; } = DateTime.Now;

      

         
            public User User { get; set; }
        }

    }

