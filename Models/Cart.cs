using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }   
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ProductName
        {
            get;
            set;
        }
        public string ProductId
        {
            get; set;
        }
        public string Price { get; set; }
      
        public int Qty { get; set; }
        public string Image { get; set; }
      


      
    }
}
