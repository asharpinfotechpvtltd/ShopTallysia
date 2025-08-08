using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceTelysia.Models
{
    public class Inventory
    {
    
   
        [Key]
        public int InventoryId { get; set; }
        [ForeignKey("Product")]

        public int ProductId { get; set; }
        public string SKU { get; set; }

        public string ProductName { get; set; }

        public int TotalQuantity { get; set; } 

        [ForeignKey("TblWarehouseMaster")]
        public int WarehouseId { get; set; }

        public WareHouse TblWarehouseMaster { get; set; }
        public Products Product { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.Now;


    } }
