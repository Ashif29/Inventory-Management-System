using InventoryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface IPurchaseOrderService
    {
        Task<bool> AddAsync(PurchaseOrder purchaseOrder);
        Task<IEnumerable<PurchaseOrder>> GetAllAsync(Expression<Func<PurchaseOrder, bool>>? filter, string? includeProperties = null);
        Task<int> GetCountAsync(Expression<Func<PurchaseOrder, bool>>? filter = null);
        Task<PurchaseOrder> GetByIdAsync(Expression<Func<PurchaseOrder, bool>> filter, string? includeProperties = null);
        Task<PurchaseOrder> OrderDetails(Guid OrderId);
        Task<bool> UpdateAsync(PurchaseOrder purchaseOrder);
    }
}
