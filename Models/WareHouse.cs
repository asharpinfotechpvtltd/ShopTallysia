using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class WareHouse
    {


        [Key]
        public int WarehouseId { get; set; }

 
        public string WarehouseName { get; set; }

        public string Location { get; set; }

       


    }
}