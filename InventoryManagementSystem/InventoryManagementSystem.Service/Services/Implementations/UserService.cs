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
    using InventoryManagementSystem.Data.Entities.NotMapped;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersWithRolesAsync()
        {
            var users = _userManager.Users.ToList();
            var usersDtoList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                string userRole = roles.FirstOrDefault();

                // Assign a default role if none exists
                if (string.IsNullOrEmpty(userRole))
                {
                    userRole = UserRoles.None; // Assign default role
                    //await _userManager.AddToRoleAsync(user, userRole); // Optional
                }

                usersDtoList.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName, // Assuming FullName is a property in ApplicationUser
                    Role = userRole
                });
            }

            return usersDtoList;
        }
    }

}
