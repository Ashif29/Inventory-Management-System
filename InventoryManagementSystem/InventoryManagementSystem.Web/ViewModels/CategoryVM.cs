using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Entities;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class CategoryVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public CategoryQueryParameters? QueryParameters { get; set; }

        public PaginatedList<Category> CategoriesPaged { get; set; }
    }
}
