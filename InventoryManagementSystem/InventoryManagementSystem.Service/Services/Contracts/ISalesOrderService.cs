using InventoryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface ISalesOrderService
    {
        Task<bool> AddAsync(SalesOrder salesOrder);
        Task<IEnumerable<SalesOrder>> GetAllAsync(Expression<Func<SalesOrder, bool>>? filter, string? includeProperties = null);
        Task<int> GetCountAsync(Expression<Func<SalesOrder, bool>>? filter = null);
        Task<SalesOrder> OrderDetails(Guid OrderId);
        Task<SalesOrder> GetByIdAsync(Expression<Func<SalesOrder, bool>> filter, string? includeProperties = null);
        Task<bool> UpdateAsync(SalesOrder salesOrder);
    }
}
