using ImageMagick;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Repositories.Core;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddAsync(Category category)
        {
            await _unitOfWork.CategoryRepository.AddAsync(category);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(e => e.Id == id);

            if (category == null)
            {
                return false;
            }

            await _unitOfWork.CategoryRepository.DeleteAsync(category);

            return await _unitOfWork.CompleteAsync();
        }

        public async Task<CategoryData> GetAllAsync(CategoryQueryParameters? queryParameters, int pageNumber, int pageSize)
        {
            var filter = ApplySearching(queryParameters);

            // Retrieve filtered categories
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(filter);

            // Apply sorting
            categories = ApplySorting(categories, queryParameters);

            // Handle pagination
            var pagedCategories = await PaginatedList<Category>.CreateAsync(categories, pageNumber, pageSize);

            return new CategoryData
            {
                categories = categories,
                pagedCategories = pagedCategories
            };
        }

        public async Task<Category> GetByIdAsync(Expression<Func<Category, bool>> filter)
        {
            return await _unitOfWork.CategoryRepository.GetByIdAsync(filter);
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            await _unitOfWork.CategoryRepository.UpdateAsync(category);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> IsExistsAsync(Expression<Func<Category, bool>> filter)
        {
            return await _unitOfWork.CategoryRepository.IsExistsAsync(filter);
        }

        private Expression<Func<Category, bool>> ApplySearching(CategoryQueryParameters? queryParameters)
        {
            return e =>
                (string.IsNullOrEmpty(queryParameters.Name) ||
                 (e.Name).Contains(queryParameters.Name));
        }

        private IQueryable<Category> ApplySorting(IQueryable<Category> categories, CategoryQueryParameters queryParameters)
        {
            switch (queryParameters.SortColumn)
            {
                case "name":
                    categories = queryParameters.SortOrder == "dsc"
                        ? categories.OrderByDescending(e => e.Name)
                        : categories.OrderBy(e => e.Name);
                    break;
                default:
                    break;
            }

            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _unitOfWork.CategoryRepository.GetAllAsync();
        }
    }
}
