using InventoryManagementSystem.Data.DataAccess;
using InventoryManagementSystem.Data.Repositories.Contracts;
using InventoryManagementSystem.Data.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Repositories.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _db;

        public IProductRepository ProductRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public ISupplierRepository SupplierRepository { get; private set; }
        public IPurchaserRepository PurchaserRepository { get; private set; }
        public IPurchaseOrderRepository PurchaseOrderRepository { get; private set; }
        public IPurchaseOrderDetailRepository PurchaseOrderDetailRepository { get; private set; }

        public IConsumerRepository ConsumerRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            this.ProductRepository = new ProductRepository(_db);
            this.CategoryRepository = new CategoryRepository(_db);
            this.SupplierRepository = new SupplierRepository(_db);
            this.PurchaserRepository = new PurchaserRepository(_db);
            this.PurchaseOrderRepository = new PurchaseOrderRepository(_db);
            this.PurchaseOrderDetailRepository = new PurchaseOrderDetailRepository(_db);
            this.ConsumerRepository = new ConsumerRepository(_db);
        }

        public async Task<bool> CompleteAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
