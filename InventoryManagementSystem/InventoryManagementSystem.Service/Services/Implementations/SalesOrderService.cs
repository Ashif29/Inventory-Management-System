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
    public class SalesOrderService : ISalesOrderService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public SalesOrderService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(SalesOrder salesOrder)
        {
            await _unitOfWork.SalesOrderRepository.AddAsync(salesOrder);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<SalesOrder>> GetAllAsync(Expression<Func<SalesOrder, bool>>? filter, string? includeProperties = null)
        {
            return await _unitOfWork.SalesOrderRepository.GetAllAsync(filter, includeProperties);
        }
        public async Task<int> GetCountAsync(Expression<Func<SalesOrder, bool>>? filter = null)
        {
            return await _unitOfWork.SalesOrderRepository.CountAsync(filter);
        }
        public async Task<SalesOrder> OrderDetails(Guid OrderId)
        {
            return await _unitOfWork.SalesOrderRepository.OrderDetails(OrderId);
        }
        public Task<SalesOrder> GetByIdAsync(Expression<Func<SalesOrder, bool>> filter, string? includeProperties = null)
        {
            return _unitOfWork.SalesOrderRepository.GetByIdAsync(filter, includeProperties);
        }
        public async Task<bool> UpdateAsync(SalesOrder salesOrder)
        {
            await _unitOfWork.SalesOrderRepository.UpdateAsync(salesOrder);
            return await _unitOfWork.CompleteAsync();
        }
    }
}
