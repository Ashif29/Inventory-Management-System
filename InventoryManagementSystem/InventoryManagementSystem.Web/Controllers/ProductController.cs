using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(ProductQueryParameters? queryParameters, int pageNumber)
        {
            if (pageNumber < 1) pageNumber = 1;

            int pageSize = 5;

            var productsData = await _productService.GetAllAsync(queryParameters, pageNumber, pageSize);

            var productVM = new ProductVM
            {
                Products = productsData.products,
                QueryParameters = queryParameters,
                ProductsPaged = productsData.pagedProducts
            };

            return View(productVM);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (ModelState.IsValid)
            {
                bool IsNameExists = await _productService.IsExistsAsync(u => u.Name == product.Name);

                if (IsNameExists)
                {
                    TempData["error"] = "This name already exists!";
                    return View();
                }

                var success = await _productService.AddAsync(product, product.Image);

                if (success)
                {
                    TempData["success"] = "The Product added successfully.";
                    return RedirectToAction("Index");
                }
            }

            TempData["error"] = "An error occurs while adding.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _productService.DeleteAsync(id);

            if (!success)
            {
                TempData["error"] = "An error occurs while deleting.";
                return NotFound();
            }

            TempData["success"] = "The Product has been deleted successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid ProductId)
        {
            var product = await _productService.GetByIdAsync(u => u.Id == ProductId);

            if (product == null)
            {
                TempData["error"] = "Product not found.";
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var success = await _productService.UpdateAsync(product);

                if (success)
                {
                    TempData["success"] = "The Product updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            TempData["error"] = "An error occurs while updating.";
            return RedirectToAction("Index");
        }

    }
}
