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
    public class ConsumerService : IConsumerService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public ConsumerService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(string userId)
        {
            var existingConsumer = await _unitOfWork.ConsumerRepository.GetByIdAsync(u => u.UserId == userId);
            if (existingConsumer != null)
            {
                return false;
            }

            var consumerFormIdentity = await _userManager.FindByIdAsync(userId);

            var consumer = new Consumer
            {
                UserId = consumerFormIdentity.Id,
                FullName = consumerFormIdentity.FullName,
                Email = consumerFormIdentity.Email 
            };

            await _unitOfWork.ConsumerRepository.AddAsync(consumer);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<Consumer>> GetAllAsync()
        {
            return await _unitOfWork.ConsumerRepository.GetAllAsync();
        }
        public async Task<Consumer> GetByIdAsync(Expression<Func<Consumer, bool>> filter)
        {
            return await _unitOfWork.ConsumerRepository.GetByIdAsync(filter);
        }
    }
}
