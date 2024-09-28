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
    public class SalesmanRepository : Repository<Salesman>, ISalesmanRepository
    {

        private readonly ApplicationDbContext _db;
        public SalesmanRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
