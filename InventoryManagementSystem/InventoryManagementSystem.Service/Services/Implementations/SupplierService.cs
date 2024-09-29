using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Core;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Implementations
{
    public class SupplierService : ISupplierService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(string userId)
        {
            var existingSupplier = await _unitOfWork.SupplierRepository.GetByIdAsync(u => u.UserId == userId);
            if (existingSupplier != null)
            {
                return false;
            }

            var supplierFormIdentity = await _userManager.FindByIdAsync(userId);

            var supplier = new Supplier
            {
                UserId = supplierFormIdentity.Id,
                FullName = supplierFormIdentity.FullName,
                Email = supplierFormIdentity.Email 
            };

            await _unitOfWork.SupplierRepository.AddAsync(supplier);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _unitOfWork.SupplierRepository.GetAllAsync();
        }

        public async Task<Supplier> GetByIdAsync(Expression<Func<Supplier, bool>> filter)
        {
            return await _unitOfWork.SupplierRepository.GetByIdAsync(filter);
        }
    }
}
