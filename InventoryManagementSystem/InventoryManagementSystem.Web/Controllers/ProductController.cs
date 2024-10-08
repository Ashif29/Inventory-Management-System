﻿using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger; 
        }

        public async Task<IActionResult> Index(ProductQueryParameters? queryParameters, int pageNumber)
        {
            if (pageNumber < 1) pageNumber = 1;

            int pageSize = 5;

            _logger.LogInformation("Fetching products with page number {PageNumber}", pageNumber); 

            var productsData = await _productService.GetAllAsync(queryParameters, pageNumber, pageSize, includeProperties: "Category");

            var productVM = new ProductVM
            {
                Products = productsData.products,
                QueryParameters = queryParameters,
                ProductsPaged = productsData.pagedProducts
            };

            return View(productVM);
        }

        public async Task<IActionResult> Add()
        {
            var categoryList = await _categoryService.GetAllCategoryAsync();

            _logger.LogInformation("Loading add product view."); 

            var productVM = new ProductVM
            {
                Categories = categoryList.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Product = new Product()
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductVM productVM)
        {
            productVM.Product.Category = await _categoryService.GetByIdAsync(u => u.Id == productVM.Product.CategoryId);

            if (ModelState.IsValid)
            {
                bool IsNameExists = await _productService.IsExistsAsync(u => u.Name == productVM.Product.Name);

                if (IsNameExists)
                {
                    TempData["error"] = "This name already exists!";
                    _logger.LogWarning("Product name already exists: {ProductName}", productVM.Product.Name); 
                    return View();
                }

                var success = await _productService.AddAsync(productVM.Product, productVM.Product.Image);

                if (success)
                {
                    TempData["success"] = "The Product added successfully.";
                    _logger.LogInformation("Product added successfully: {ProductName}", productVM.Product.Name); 
                    return RedirectToAction("Index");
                }
            }

            TempData["error"] = "An error occurs while adding.";
            _logger.LogError("Error occurred while adding product: {ProductName}", productVM.Product.Name); 

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _productService.DeleteAsync(id);

            if (!success)
            {
                TempData["error"] = "An error occurs while deleting.";
                _logger.LogError("Error occurred while deleting product with ID: {ProductId}", id);
                return NotFound();
            }

            TempData["success"] = "The Product has been deleted successfully.";
            _logger.LogInformation("Product deleted successfully with ID: {ProductId}", id); 

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid ProductId)
        {
            var product = await _productService.GetByIdAsync(u => u.Id == ProductId, includeProperties: "Category");

            if (product == null)
            {
                TempData["error"] = "Product not found.";
                _logger.LogWarning("Product not found with ID: {ProductId}", ProductId);
                return NotFound();
            }

            var categoryList = await _categoryService.GetAllCategoryAsync();

            _logger.LogInformation("Loading edit product view for product ID: {ProductId}", ProductId); 

            var productVM = new ProductVM
            {
                Categories = categoryList.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Product = product
            };
            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var success = await _productService.UpdateAsync(productVM.Product);

                if (success)
                {
                    TempData["success"] = "The Product updated successfully.";
                    _logger.LogInformation("Product updated successfully: {ProductName}", productVM.Product.Name); 
                    return RedirectToAction("Index");
                }
            }

            TempData["error"] = "An error occurs while updating.";
            _logger.LogError("Error occurred while updating product: {ProductName}", productVM.Product.Name); 

            return RedirectToAction("Index");
        }
    }

}
