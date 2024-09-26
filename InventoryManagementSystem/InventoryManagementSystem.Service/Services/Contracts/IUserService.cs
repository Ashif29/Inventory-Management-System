using InventoryManagementSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync(string? includeProperties = null);
    }
}
