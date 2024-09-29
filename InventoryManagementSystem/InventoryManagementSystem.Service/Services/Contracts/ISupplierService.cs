using InventoryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface ISupplierService
    {
        Task<bool> AddAsync(string  userId);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> GetByIdAsync(Expression<Func<Supplier, bool>> filter);
    }
}
