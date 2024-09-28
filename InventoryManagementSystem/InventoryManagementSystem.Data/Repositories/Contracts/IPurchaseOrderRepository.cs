using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Repositories.Contracts
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
    {
        Task<PurchaseOrder> OrderDetails(Guid OrderId);
    }
}
