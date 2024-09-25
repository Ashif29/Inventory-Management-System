using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface ICategoryService
    {
        Task<CategoryData> GetAllAsync(CategoryQueryParameters? queryParameters, int pageNumber, int pageSize);
        Task<Category> GetByIdAsync(Expression<Func<Category, bool>> filter);
        Task<bool> IsExistsAsync(Expression<Func<Category, bool>> filter);
        Task<bool> AddAsync(Category Category);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(Category Category);
    }
}
