using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Entities.NotMapped
{
    public class ProductQueryParameters
    {
        // Searching Query Property
        public string? Name { get; set; }

        // Sorting Query Property
        public string SortOrder { get; set; } = "asc";
        public string SortColumn { get; set; } = "created";
    }
}
