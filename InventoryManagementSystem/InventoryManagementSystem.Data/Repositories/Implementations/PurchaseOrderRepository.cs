using InventoryManagementSystem.Data.DataAccess;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Contracts;
using InventoryManagementSystem.Data.Repositories.Core;
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
    }
}
