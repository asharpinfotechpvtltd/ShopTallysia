using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class Reviews
    {
        [Key]
        public int Id { get; set; }
        public string comment { get; set; }
     
        public string email { get; set; }   
        public DateTime Date { get; set; }
        public int review { get; set; }
        public string ProductId { get; set; }
        public bool Isviewable { get; set; }
    }
}
