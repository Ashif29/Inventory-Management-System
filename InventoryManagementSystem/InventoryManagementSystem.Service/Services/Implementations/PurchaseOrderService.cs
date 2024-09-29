using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Core;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Implementations
{
    public class PurchaseOrderService : IPurchaseOrderService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public PurchaseOrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(PurchaseOrder purchaseOrder)
        {
            await _unitOfWork.PurchaseOrderRepository.AddAsync(purchaseOrder);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllAsync(Expression<Func<PurchaseOrder, bool>>? filter, string? includeProperties = null)
        {
            return await _unitOfWork.PurchaseOrderRepository.GetAllAsync(filter, includeProperties);
        }

        public Task<PurchaseOrder> GetByIdAsync(Expression<Func<PurchaseOrder, bool>> filter, string? includeProperties = null)
        {
            return _unitOfWork.PurchaseOrderRepository.GetByIdAsync(filter, includeProperties);
        }

        public async Task<int> GetCountAsync(Expression<Func<PurchaseOrder, bool>>? filter = null)
        {
            return await _unitOfWork.PurchaseOrderRepository.CountAsync(filter);
        }

        public async Task<PurchaseOrder> OrderDetails(Guid OrderId)
        {
            return await _unitOfWork.PurchaseOrderRepository.OrderDetails(OrderId);
        }

        public async Task<bool> UpdateAsync(PurchaseOrder purchaseOrder)
        {
           await _unitOfWork.PurchaseOrderRepository.UpdateAsync(purchaseOrder);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
