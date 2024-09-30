using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Enums;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Service.Services.Implementations;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize]
    public class SalesOrderController : Controller
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ISalesOrderDetailService _salesOrderDetailService;
        private readonly IProductService _productService;
        private readonly IConsumerService _consumerService;
        private readonly ISalesmanService _salesmanService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SalesOrderController> _logger;

        public SalesOrderController(
            ISalesOrderService salesOrderService,
            ISalesOrderDetailService salesOrderDetailService,
            IProductService productService,
            IConsumerService consumerService,
            ISalesmanService salesmanService,
            UserManager<ApplicationUser> userManager,
            ILogger<SalesOrderController> logger)
        {
            _salesOrderService = salesOrderService;
            _salesOrderDetailService = salesOrderDetailService;
            _productService = productService;
            _consumerService = consumerService;
            _salesmanService = salesmanService;
            _userManager = userManager;
            _logger = logger;
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Index()
        {
            var salesOrders = await _salesOrderService.GetAllAsync(null, includeProperties: "Salesman,Consumer");
            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            _logger.LogInformation("Retrieved all sales orders for admin.");

            return View(model);
        }

        [Authorize(Roles = UserRoles.Salesman)]
        public async Task<IActionResult> Add()
        {
            
            var model = new SalesOrderVM();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = await _userManager.FindByIdAsync(userId);

            model.ConsumersList = (await _consumerService.GetAllAsync())
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.FullName 
                });

            model.CurrentSalesmanName = applicationUser?.FullName;
            model.nextSOCode = "SO-" + (await _salesOrderService.GetCountAsync() + 1);
            _logger.LogInformation("Salesman {SalesmanName} accessed the add sales order page.", model.CurrentSalesmanName);

            return View(model);
        }

        [Authorize(Roles = UserRoles.Salesman)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(SalesOrderVM model)
        {
            if (ModelState.IsValid)
            {

                Salesman salesman = await _salesmanService.GetByIdAsync(u => u.FullName == model.CurrentSalesmanName);
                
                var salesOrder = model.SalesOrder;

                salesOrder.SOCode = model.nextSOCode;
                salesOrder.SalesmanId = salesman.Id;
                salesOrder.CreatedAt = DateTime.Now;
                salesOrder.ConsumerId = model.SalesOrder.ConsumerId;
                salesOrder.Notes = model.SalesOrder.Notes;

                var result = await _salesOrderService.AddAsync(salesOrder);

                if (result)
                {
                    foreach (var detail in model.SalesOrderDetailItems)
                    {
                        detail.SalesOrderId = salesOrder.Id;
                        await _salesOrderDetailService.AddAsync(detail);
                    }
                    _logger.LogInformation("Sales order {SOCode} added successfully.", salesOrder.SOCode);

                    return RedirectToAction("Index", "Salesman");
                }
                _logger.LogError("Error adding sales order. Model state: {ModelState}", ModelState);

                ModelState.AddModelError("", "Error adding sales order.");
            }
            return RedirectToAction("Index", "Salesman");
        }

        [Authorize(Roles = UserRoles.Salesman)]
        public async Task<IActionResult> GetProductList()
        {
            var ProductList = (await _productService.GetAllAsync())
               .Select(p => new SelectListItem
               {
                   Value = p.Id.ToString(),
                   Text = p.Name
               });
            _logger.LogInformation("Retrieved product list for sales order.");

            return Ok(ProductList);
        }


        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Salesman + "," + UserRoles.Consumer)]
        public async Task<IActionResult> OrderDetails(Guid OrderId)
        {
            var orderDetails = await _salesOrderService.OrderDetails(OrderId);

            var model = new SalesOrderDetailsVM
            {
                Id = orderDetails.Id,
                SOCode = orderDetails.SOCode,
                SalesmanName = orderDetails.Salesman.FullName,
                SalesmanEmail = orderDetails.Salesman.Email,
                ConsumerName = orderDetails.Consumer.FullName,
                ConsumerEmail = orderDetails.Consumer.Email,
                DeliveryDate = orderDetails.DeliveryDate,
                Status = orderDetails.Status,
                Notes = orderDetails.Notes,
                TotalCost = orderDetails.TotalCost,
                SalesOrderItems = orderDetails.SalesOrderDetails.Select(detail => new SalesOrderItemVM
                {
                    ProductName = detail.Product.Name,
                    SalesPrice = detail.SalesPrice,
                    Quantity = detail.Quantity
                }).ToList()
            };
            _logger.LogInformation("Viewed order details for sales order {SOCode}.", orderDetails.SOCode);

            return View(model);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Salesman + "," + UserRoles.Consumer)]
        public async Task<IActionResult> GenerateInvoicePdf(Guid OrderId)
        {
            var orderDetails = await _salesOrderService.OrderDetails(OrderId);

            var model = new SalesOrderDetailsVM
            {
                SOCode = orderDetails.SOCode,
                SalesmanName = orderDetails.Salesman.FullName,
                SalesmanEmail = orderDetails.Salesman.Email,
                ConsumerName = orderDetails.Consumer.FullName,
                ConsumerEmail = orderDetails.Consumer.Email,
                DeliveryDate = orderDetails.DeliveryDate,
                Status = orderDetails.Status,
                Notes = orderDetails.Notes,
                TotalCost = orderDetails.TotalCost,
                SalesOrderItems = orderDetails.SalesOrderDetails.Select(detail => new SalesOrderItemVM
                {
                    ProductName = detail.Product.Name,
                    SalesPrice = detail.SalesPrice,
                    Quantity = detail.Quantity
                }).ToList()
            };
            _logger.LogInformation("Generating invoice PDF for sales order {SOCode}.", orderDetails.SOCode);

            return View(model);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> PendingSO()
        {
            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.Status == OrderStatus.Pending,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;
            _logger.LogInformation("Viewed pending sales orders.");
            return View(model);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> VerifiedSO()
        {
            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.Status == OrderStatus.Verified,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;
            _logger.LogInformation("Viewed verified sales orders.");
            return View(model);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CanceledSO()
        {
            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.Status == OrderStatus.Canceled,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;
            _logger.LogInformation("Viewed canceled sales orders.");
            return View(model);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeliveredSO()
        {
            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.Status == OrderStatus.Delivered,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;
            _logger.LogInformation("Viewed delivered sales orders.");
            return View(model);
        }

    }
}
