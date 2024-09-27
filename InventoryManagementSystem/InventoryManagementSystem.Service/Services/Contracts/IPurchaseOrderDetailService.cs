using InventoryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface IPurchaseOrderDetailService
    {
        Task<bool> AddAsync(PurchaseOrderDetail purchaseOrderDetail);
    }
}
