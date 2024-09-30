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

        public SalesOrderController(
            ISalesOrderService salesOrderService,
            ISalesOrderDetailService salesOrderDetailService,
            IProductService productService,
            IConsumerService consumerService,
            ISalesmanService salesmanService,
            UserManager<ApplicationUser> userManager)
        {
            _salesOrderService = salesOrderService;
            _salesOrderDetailService = salesOrderDetailService;
            _productService = productService;
            _consumerService = consumerService;
            _salesmanService = salesmanService;
            _userManager = userManager;
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> Index()
        {
            var salesOrders = await _salesOrderService.GetAllAsync(null, includeProperties: "Salesman,Consumer");
            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

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
            model.nextSOCode = "SO-100" + (await _salesOrderService.GetCountAsync() + 1);
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
                // Create a SalesOrder instance from the model
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
                    return RedirectToAction("Index", "Salesman");
                }
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

            return View(model);
        }

        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> CanceiledSO()
        {
            var salesOrders = (await _salesOrderService.GetAllAsync(
                                u => u.Status == OrderStatus.Canceiled,
                                includeProperties: "Salesman,Consumer"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

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

            return View(model);
        }

    }
}
