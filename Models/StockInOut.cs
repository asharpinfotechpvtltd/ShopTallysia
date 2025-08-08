using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class StockInOut
    {


        [Key]
        public int LogId { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }
        public string SKU { get; set; }

        public string Type { get; set; } 

        public int Quantity { get; set; }

        public DateTime ActionDate { get; set; } = DateTime.Now;

     

        [ForeignKey("TblWarehouseMaster")]
        public int WarehouseId { get; set; }

        public WareHouse TblWarehouseMaster { get; set; }
        public Products Products { get; set; }


    }
}