using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class InternalNote
    {
        [Key]
        public int NoteId { get; set; }

        [ForeignKey("Sender")]
        public int SenderUserId { get; set; }

        [ForeignKey("Receiver")]
        public int ReceiverUserId { get; set; }

        public string NoteText { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User Sender { get; set; }

        public User Receiver { get; set; }
    }

}
