using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Enums;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Service.Services.Implementations;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = UserRoles.Salesman)]
    public class SalesmanController : Controller
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ISalesOrderDetailService _salesOrderDetailService;
        private readonly IProductService _productService;
        private readonly IConsumerService _consumerService;
        private readonly ISalesmanService _salesmanService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SalesmanController> _logger;

        public SalesmanController(
            ISalesOrderService salesOrderService,
            ISalesOrderDetailService salesOrderDetailService,
            IProductService productService,
            IConsumerService consumerService,
            ISalesmanService salesmanService,
            UserManager<ApplicationUser> userManager,
            ILogger<SalesmanController> logger)
        {
            _salesOrderService = salesOrderService;
            _salesOrderDetailService = salesOrderDetailService;
            _productService = productService;
            _consumerService = consumerService;
            _salesmanService = salesmanService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Salesman salesman = await _salesmanService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.SalesmanId == salesman.Id,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Salesman {SalesmanId} accessed the Sales Order Index.", salesman.Id); 

            return View(model);
        }

        public async Task<IActionResult> PendingSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Salesman salesman = await _salesmanService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.SalesmanId == salesman.Id && u.Status == OrderStatus.Pending,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Salesman {SalesmanId} accessed Pending Sales.", salesman.Id);

            return View(model);
        }

        public async Task<IActionResult> VerifiedSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Salesman salesman = await _salesmanService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.SalesmanId == salesman.Id && u.Status == OrderStatus.Verified,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Salesman {SalesmanId} accessed Verified Sales.", salesman.Id); 

            return View(model);
        }

        public async Task<IActionResult> CanceledSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Salesman salesman = await _salesmanService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.SalesmanId == salesman.Id && u.Status == OrderStatus.Canceled,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Salesman {SalesmanId} accessed Canceled Sales.", salesman.Id); 

            return View(model);
        }

        public async Task<IActionResult> DeliveredSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Salesman salesman = await _salesmanService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.SalesmanId == salesman.Id && u.Status == OrderStatus.Delivered,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Salesman {SalesmanId} accessed Delivered Sales.", salesman.Id); 

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CancelSO(Guid id)
        {
            var salesOrder = await _salesOrderService.GetByIdAsync(u => u.Id == id);

            if (salesOrder == null)
            {
                _logger.LogWarning("Sales order {OrderId} not found for cancellation attempt.", id); 
                return NotFound("Sales order not found.");
            }

            salesOrder.Status = OrderStatus.Canceled;

            var success = await _salesOrderService.UpdateAsync(salesOrder);

            if (success)
            {
                TempData["success"] = "The Sales order canceled.";
                _logger.LogInformation("Sales order {OrderId} has been canceled.", id);
                return RedirectToAction("Index");
            }

            TempData["error"] = "Cancellation Error.";
            _logger.LogError("Failed to cancel sales order {OrderId}.", id); 

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeliverSO(Guid id)
        {
            var salesOrder = await _salesOrderService.GetByIdAsync(u => u.Id == id);

            if (salesOrder == null)
            {
                _logger.LogWarning("Sales order {OrderId} not found for delivery attempt.", id);
                return NotFound("Sales order not found.");
            }

            salesOrder.Status = OrderStatus.Delivered;

            var success = await _salesOrderService.UpdateAsync(salesOrder);

            if (success)
            {
                TempData["success"] = "The Sales order delivered.";
                _logger.LogInformation("Sales order {OrderId} has been delivered.", id); 
                return RedirectToAction("Index");
            }

            TempData["error"] = "Delivery Error.";
            _logger.LogError("Failed to deliver sales order {OrderId}.", id); 

            return RedirectToAction(nameof(Index));
        }
    }

}
