using InventoryManagementSystem.Data.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementSystem.Data.Enums;

namespace InventoryManagementSystem.Data.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        [Required]
        public Guid PurchaserId { get; set; }

        [ForeignKey("PurchaserId")]
        public Purchaser Purchaser { get; set; }

        [Required]
        public Guid SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; } 

        public string? Notes { get; set; }

        public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } // Navigation property for details
    }
}
