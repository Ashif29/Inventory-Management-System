using InventoryManagementSystem.Data.Entities.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class SalesOrder : BaseEntity
    {
        [Required]
        [ValidateNever]
        public string SOCode { get; set; }

        [Required]
        public Guid SalesmanId { get; set; }

        [ForeignKey("SalesmanId")]
        [ValidateNever]
        public Salesman Salesman { get; set; }

        [Required]
        public Guid ConsumerId { get; set; }

        [ForeignKey("ConsumerId")]
        [ValidateNever]
        public Consumer Consumer { get; set; }

        public DateTime? DeliveryDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public string? Notes { get; set; }

        public double TotalCost { get; set; }

        [ValidateNever]
        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } // Navigation property for order details
    }

}
