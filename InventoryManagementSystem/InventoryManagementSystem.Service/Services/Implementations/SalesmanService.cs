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
    public class SalesmanService : ISalesmanService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;

        public SalesmanService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<bool> AddAsync(string userId)
        {
            var existingSalesman = await _unitOfWork.SalesmanRepository.GetByIdAsync(u => u.UserId == userId);
            if (existingSalesman != null)
            {
                return false;
            }

            var salesmanFormIdentity = await _userManager.FindByIdAsync(userId);

            var salesman = new Salesman
            {
                UserId = salesmanFormIdentity.Id,
                FullName = salesmanFormIdentity.FullName,
                Email = salesmanFormIdentity.Email 
            };

            await _unitOfWork.SalesmanRepository.AddAsync(salesman);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<Salesman> GetByIdAsync(Expression<Func<Salesman, bool>> filter)
        {
            return await _unitOfWork.SalesmanRepository.GetByIdAsync(filter);  
        }
    }
}
