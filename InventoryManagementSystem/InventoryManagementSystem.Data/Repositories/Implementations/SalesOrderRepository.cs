using InventoryManagementSystem.Data.DataAccess;
using InventoryManagementSystem.Data.Entities;
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
    public class SalesOrderRepository : Repository<SalesOrder>, ISalesOrderRepository
    {

        private readonly ApplicationDbContext _db;
        public SalesOrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<SalesOrder> OrderDetails(Guid OrderId)
        {
            var salesOrder = await _db.SalesOrders
            .Include(po => po.SalesOrderDetails)
            .ThenInclude(pod => pod.Product)
            .Include(po => po.Salesman)
            .Include(po => po.Consumer)
            .FirstOrDefaultAsync(po => po.Id == OrderId);

            return salesOrder;
        }
    }
}
