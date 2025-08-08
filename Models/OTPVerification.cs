using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class OTPVerification
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public string MobileNo { get; set; }
        public DateTime Date { get; set; }
    }
}
