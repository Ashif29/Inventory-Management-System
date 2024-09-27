using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementSystem.Data.Entities.Core;

namespace InventoryManagementSystem.Data.Entities
{
    public class Supplier : BaseEntity
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Navigation property to the ApplicationUser
        public virtual ApplicationUser User { get; set; }
    }
}
