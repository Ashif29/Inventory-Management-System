using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Entities.NotMapped
{
    public class CategoryData
    {
        public IQueryable<Category> categories { get; set; }
        public PaginatedList<Category> pagedCategories { get; set; }
    }
}
