using InventoryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface IPurchaserService
    {
        Task<bool> AddAsync(string  userId);
        Task<Purchaser> GetByIdAsync(Expression<Func<Purchaser, bool>> filter);
    }
}
