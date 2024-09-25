using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class ProductVM
    {
        public IEnumerable<Product> Products { get; set; }
        public ProductQueryParameters? QueryParameters { get; set; }

        public PaginatedList<Product> ProductsPaged { get; set; }
    }
}
