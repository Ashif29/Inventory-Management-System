using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace InventoryManagementSystem.Web.Controllers
{
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

        public async Task<IActionResult> Index()
        {
            var salesOrders = await _salesOrderService.GetAllAsync(null, includeProperties: "Salesman,Consumer");
            var model = new SalesOrderVM();
            model.SalesOrderList = salesOrders;

            return View(model);
        }

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
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error adding sales order.");
            }
            return View(model);
        }


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

       
    }
}
