using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Entities.NotMapped
{
    public class ProductsData
    {
        public IQueryable<Product> products { get; set; }
        public PaginatedList<Product> pagedProducts { get; set; }
    }
}
