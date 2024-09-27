using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Core;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Implementations
{
    public class PurchaseOrderDetailService : IPurchaseOrderDetailService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderDetailService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(PurchaseOrderDetail purchaseOrderDetail)
        {
            await _unitOfWork.PurchaseOrderDetailRepository.AddAsync(purchaseOrderDetail);
            return await _unitOfWork.CompleteAsync();

        }
    }
}
