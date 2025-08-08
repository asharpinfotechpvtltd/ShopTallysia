using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string skucode { get; set; }
        public bool Visible { get; set; }
    }
}
