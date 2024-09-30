using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagementSystem.Web.Controllers
{

    [Authorize(Roles = UserRoles.Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(CategoryQueryParameters? queryParameters, int pageNumber)
        {
            if (pageNumber < 1) pageNumber = 1;

            int pageSize = 5;

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
                bool IsNameExists = await _categoryService.IsExistsAsync(u => u.Name == category.Name);

                if (IsNameExists)
                {
                    TempData["error"] = "This name already exists!";
                    return View();
                }

                var success = await _categoryService.AddAsync(category);

                if (success)
                {
                    TempData["success"] = "The Category added successfully.";
                    return RedirectToAction("Index");
                }
            }

            TempData["error"] = "An error occurs while adding.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _categoryService.DeleteAsync(id);

            if (!success)
            {
                TempData["error"] = "An error occurs while deleting.";
                return NotFound();
            }

            TempData["success"] = "The Category has been deleted successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid CategoryId)
        {
            var category = await _categoryService.GetByIdAsync(u => u.Id == CategoryId);

            if (category == null)
            {
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
                    TempData["success"] = "The Category updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            TempData["error"] = "An error occurs while updating.";
            return RedirectToAction("Index");
        }
    }
}
