using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Service.Services.Implementations;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace InventoryManagementSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISupplierService _supplierService;

        public UserController(IUserService userService, ISupplierService supplierService)
        {
            _userService = userService;
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersWithRolesAsync();

            var rolesList = new List<string>
            {
                UserRoles.Admin,
                UserRoles.Salesman,
                UserRoles.Purchaser,
                UserRoles.Supplier,
                UserRoles.Consumer,
                UserRoles.None
            };

            var userVM = new UserVM
            {
                Users = users,
                RolesList = rolesList.Select(roleName => new SelectListItem
                {
                    Value = roleName,
                    Text = roleName
                })
            };

            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var result = await _userService.AssignRoleToUserAsync(userId, role);
            if (!result)
            {
                return NotFound();
            }

            bool isAdded = await AddUserToRelatedTableAsync(userId, role);

            /*if (!isAdded)
            {
                return BadRequest("User could not be added.");
            }*/

            return Ok();
        }

        private async Task<bool> AddUserToRelatedTableAsync(string userId, string role)
        {
            if (role == UserRoles.Supplier)
            {
                return await _supplierService.AddAsync(userId);
            }
            return false;
                
        }
    }

}
