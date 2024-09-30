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
    [Authorize(Roles = UserRoles.Consumer)]
    public class ConsumerController : Controller
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ISalesOrderDetailService _salesOrderDetailService;
        private readonly IProductService _productService;
        private readonly IConsumerService _consumerService;
        private readonly ISalesmanService _salesmanService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ConsumerController> _logger; 

        public ConsumerController(
            ISalesOrderService salesOrderService,
            ISalesOrderDetailService salesOrderDetailService,
            IProductService productService,
            IConsumerService consumerService,
            ISalesmanService salesmanService,
            UserManager<ApplicationUser> userManager,
            ILogger<ConsumerController> logger) 
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
            Consumer consumer = await _consumerService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.ConsumerId == consumer.Id,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Consumer {UserId} retrieved all sales orders.", userId); 

            return View(model);
        }

        public async Task<IActionResult> PendingSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Consumer consumer = await _consumerService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.ConsumerId == consumer.Id && u.Status == OrderStatus.Pending,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Consumer {UserId} retrieved pending sales orders.", userId);

            return View(model);
        }

        public async Task<IActionResult> VerifiedSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Consumer consumer = await _consumerService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.ConsumerId == consumer.Id && u.Status == OrderStatus.Verified,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Consumer {UserId} retrieved verified sales orders.", userId); 

            return View(model);
        }

        public async Task<IActionResult> CanceledSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Consumer consumer = await _consumerService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.ConsumerId == consumer.Id && u.Status == OrderStatus.Canceled,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Consumer {UserId} retrieved canceled sales orders.", userId); 

            return View(model);
        }

        public async Task<IActionResult> DeliveredSales()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Consumer consumer = await _consumerService.GetByIdAsync(u => u.UserId == userId);

            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.ConsumerId == consumer.Id && u.Status == OrderStatus.Delivered,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Consumer {UserId} retrieved delivered sales orders.", userId); 

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifySO(Guid id)
        {
            var salesOrder = await _salesOrderService.GetByIdAsync(u => u.Id == id, includeProperties: "SalesOrderDetails,SalesOrderDetails.Product");

            if (salesOrder == null)
            {
                _logger.LogWarning("Attempted to verify a sales order that was not found. Order ID: {OrderId}", id);
                return NotFound("Sales order not found.");
            }

            salesOrder.Status = OrderStatus.Verified;

            foreach (var item in salesOrder.SalesOrderDetails)
            {
                var product = await _productService.GetByIdAsync(u => u.Id == item.ProductId);

                if (product != null)
                {
                    product.StockLevel -= item.Quantity;
                    await _productService.UpdateAsync(product);
                }
            }

            salesOrder.DeliveryDate = DateTime.Now;
            var success = await _salesOrderService.UpdateAsync(salesOrder);

            if (success)
            {
                TempData["success"] = "The Sales order verified.";
                _logger.LogInformation("Sales order {OrderId} verified successfully.", id);
                return RedirectToAction("Index");
            }

            TempData["error"] = "Verification Error.";
            _logger.LogError("Error verifying sales order {OrderId}.", id); 
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CancelSO(Guid id)
        {
            var salesOrder = await _salesOrderService.GetByIdAsync(u => u.Id == id);

            if (salesOrder == null)
            {
                _logger.LogWarning("Attempted to cancel a sales order that was not found. Order ID: {OrderId}", id);
                return NotFound("Sales order not found.");
            }

            salesOrder.Status = OrderStatus.Canceled;

            var success = await _salesOrderService.UpdateAsync(salesOrder);

            if (success)
            {
                TempData["success"] = "The Sales order canceled.";
                _logger.LogInformation("Sales order {OrderId} canceled successfully.", id);
                return RedirectToAction("Index");
            }

            TempData["error"] = "Cancelation Error.";
            _logger.LogError("Error canceling sales order {OrderId}.", id); 
            return RedirectToAction(nameof(Index));
        }
    }

}
