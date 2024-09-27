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
    public class PurchaserService : IPurchaserService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public PurchaserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(string userId)
        {
            var existingPurchaser = await _unitOfWork.PurchaserRepository.GetByIdAsync(u => u.UserId == userId);
            if (existingPurchaser != null)
            {
                return false;
            }

            var purchaserFormIdentity = await _userManager.FindByIdAsync(userId);

            var supplier = new Purchaser
            {
                UserId = purchaserFormIdentity.Id,
                FullName = purchaserFormIdentity.FullName,
                Email = purchaserFormIdentity.Email 
            };

            await _unitOfWork.PurchaserRepository.AddAsync(supplier);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<Purchaser> GetByIdAsync(Expression<Func<Purchaser, bool>> filter)
        {
            return await _unitOfWork.PurchaserRepository.GetByIdAsync(filter);  
        }
    }
}
