using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Service.Services.Implementations;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
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

            return Ok();
        }
    }

}
