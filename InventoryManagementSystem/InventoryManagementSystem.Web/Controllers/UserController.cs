using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Service.Services.Implementations;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISupplierService _supplierService;
        private readonly IPurchaserService _purchaserService;
        private readonly IConsumerService _consumerService;
        private readonly ISalesmanService _salesmanService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService, 
            ISupplierService supplierService,
            IPurchaserService purchaserService,
            IConsumerService consumerService,
            ISalesmanService salesmanService,
            ILogger<UserController> logger
            )
        {
            _userService = userService;
            _supplierService = supplierService;
            _purchaserService = purchaserService;
            _salesmanService = salesmanService;
            _consumerService = consumerService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all users with roles.");
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
            _logger.LogInformation($"Assigning role {role} to user {userId}.");

            var result = await _userService.AssignRoleToUserAsync(userId, role);
            if (!result)
            {
                _logger.LogWarning($"Failed to assign role {role} to user {userId}.");
                return NotFound();
            }

            bool isAdded = await AddUserToRelatedTableAsync(userId, role);
            _logger.LogInformation($"User {userId} successfully assigned to {role} related table: {isAdded}.");
            return Ok();
        }

        private async Task<bool> AddUserToRelatedTableAsync(string userId, string role)
        {
            if (role == UserRoles.Supplier)
            {
                return await _supplierService.AddAsync(userId);
            }
            if (role == UserRoles.Purchaser)
            {
                return await _purchaserService.AddAsync(userId);
            }
            if (role == UserRoles.Consumer)
            {
                return await _consumerService.AddAsync(userId);
            }
            if (role == UserRoles.Salesman)
            {
                return await _salesmanService.AddAsync(userId);
            }
            return false;
                
        }
    }

}
