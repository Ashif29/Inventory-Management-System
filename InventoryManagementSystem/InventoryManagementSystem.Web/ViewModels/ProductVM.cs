using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class ProductVM
    {
        [ValidateNever]
        public IEnumerable<Product> Products { get; set; }

        public Product? Product { get; set; }

        [ValidateNever]
        public List<SelectListItem> Categories { get; set; }
        
        [ValidateNever]
        public ProductQueryParameters? QueryParameters { get; set; }
        
        [ValidateNever]
        public PaginatedList<Product> ProductsPaged { get; set; }
    }
}
