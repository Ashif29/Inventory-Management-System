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
    public interface IProductService
    {
        Task<ProductsData> GetAllAsync(ProductQueryParameters? queryParameters, int pageNumber, int pageSize);
        Task<Product> GetByIdAsync(Expression<Func<Product, bool>> filter);

        Task<bool> IsExistsAsync(Expression<Func<Product, bool>> filter);
        Task<bool> AddAsync(Product product, IFormFile? imageFile);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(Product product);

    }

}
