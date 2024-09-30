using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [Authorize(Roles = UserRoles.Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(CategoryQueryParameters? queryParameters, int pageNumber)
        {
            if (pageNumber < 1) pageNumber = 1;

            int pageSize = 5;

            _logger.LogInformation("Fetching categories with page number: {PageNumber}", pageNumber);
            var categoriesData = await _categoryService.GetAllAsync(queryParameters, pageNumber, pageSize);

            var categoryVM = new CategoryVM
            {
                Categories = categoriesData.categories,
                QueryParameters = queryParameters,
                CategoriesPaged = categoriesData.pagedCategories
            };

            return View(categoryVM);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                bool isNameExists = await _categoryService.IsExistsAsync(u => u.Name == category.Name);

                if (isNameExists)
                {
                    _logger.LogWarning("Attempt to add a category with an existing name: {CategoryName}", category.Name);
                    TempData["error"] = "This name already exists!";
                    return View();
                }

                var success = await _categoryService.AddAsync(category);

                if (success)
                {
                    _logger.LogInformation("Category added successfully: {CategoryName}", category.Name);
                    TempData["success"] = "The Category added successfully.";
                    return RedirectToAction("Index");
                }
            }

            _logger.LogError("Error occurred while adding category: {CategoryName}", category?.Name);
            TempData["error"] = "An error occurs while adding.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
            var success = await _categoryService.DeleteAsync(id);

            if (!success)
            {
                _logger.LogError("Error occurred while deleting category with ID: {CategoryId}", id);
                TempData["error"] = "An error occurs while deleting.";
                return NotFound();
            }

            _logger.LogInformation("Category with ID: {CategoryId} has been deleted successfully.", id);
            TempData["success"] = "The Category has been deleted successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid categoryId)
        {
            _logger.LogInformation("Fetching category for editing with ID: {CategoryId}", categoryId);
            var category = await _categoryService.GetByIdAsync(u => u.Id == categoryId);

            if (category == null)
            {
                _logger.LogWarning("Category not found for editing with ID: {CategoryId}", categoryId);
                TempData["error"] = "Category not found.";
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var success = await _categoryService.UpdateAsync(category);

                if (success)
                {
                    _logger.LogInformation("Category updated successfully: {CategoryName}", category.Name);
                    TempData["success"] = "The Category updated successfully.";
                    return RedirectToAction("Index");
                }
            }

            _logger.LogError("Error occurred while updating category: {CategoryName}", category?.Name);
            TempData["error"] = "An error occurs while updating.";
            return RedirectToAction("Index");
        }
    }

}
