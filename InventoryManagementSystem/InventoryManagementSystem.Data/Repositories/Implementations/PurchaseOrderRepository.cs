using InventoryManagementSystem.Data.DataAccess;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Enums;
using InventoryManagementSystem.Data.Repositories.Contracts;
using InventoryManagementSystem.Data.Repositories.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Repositories.Implementations
{
    public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
    {

        private readonly ApplicationDbContext _db;
        public PurchaseOrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<PurchaseOrder> OrderDetails(Guid OrderId)
        {
            var purchaseOrder = await _db.PurchaseOrders
            .Include(po => po.PurchaseOrderDetails)
            .ThenInclude(pod => pod.Product)
            .Include(po => po.Purchaser)
            .Include(po => po.Supplier)
            .FirstOrDefaultAsync(po => po.Id == OrderId);

            return purchaseOrder;
        }
        public IQueryable<PurchaseOrderDetail> GetVerifiedPurchaseOrderDetails()
        {
            return _db.PurchaseOrders
                .Where(order => order.Status == OrderStatus.Verified)
                .SelectMany(order => order.PurchaseOrderDetails);
        }
    }
}
