using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Repositories.Core
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<bool> IsExistsAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);
        
    }
}
