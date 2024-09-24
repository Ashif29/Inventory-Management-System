﻿using InventoryManagementSystem.Data.DataAccess;
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

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            this.ProductRepository = new ProductRepository(_db);
            this.CategoryRepository = new CategoryRepository(_db);
        }

        public async Task<bool> CompleteAsync()
        {

            return await _db.SaveChangesAsync() > 0;
        }
    }
}
