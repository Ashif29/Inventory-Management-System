using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace InventoryManagementSystem.Web.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderDetailService _purchaseOrderDetailService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IPurchaserService _purchaserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaseOrderController(
            IPurchaseOrderService purchaseOrderService,
            IPurchaseOrderDetailService purchaseOrderDetailService,
            IProductService productService,
            ISupplierService supplierService,
            IPurchaserService purchaserService,
            UserManager<ApplicationUser> userManager)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseOrderDetailService = purchaseOrderDetailService;
            _productService = productService;
            _supplierService = supplierService;
            _purchaserService = purchaserService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var purchaseOrders = await _purchaseOrderService.GetAllAsync(null, includeProperties: "Purchaser,Supplier");
            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }

        public async Task<IActionResult> Add()
        {
            
            var model = new PurchaseOrderVM();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationUser = await _userManager.FindByIdAsync(userId);

            model.SuppliersList = (await _supplierService.GetAllAsync())
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.FullName 
                });

            model.CurrentPurchaserName = applicationUser?.FullName;
            model.nextPOCode = "PO-100" + (await _purchaseOrderService.GetCountAsync() + 1);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PurchaseOrderVM model)
        {
            if (ModelState.IsValid)
            {

                Purchaser purchaser = await _purchaserService.GetByIdAsync(u => u.FullName == model.CurrentPurchaserName);
                // Create a PurchaseOrder instance from the model
                var purchaseOrder = model.PurchaseOrder;

                purchaseOrder.POCode = model.nextPOCode;
                purchaseOrder.PurchaserId = purchaser.Id;
                purchaseOrder.CreatedAt = DateTime.Now;
                purchaseOrder.SupplierId = model.PurchaseOrder.SupplierId;
                purchaseOrder.Notes = model.PurchaseOrder.Notes;

                var result = await _purchaseOrderService.AddAsync(purchaseOrder);

                if (result)
                {
                    foreach (var detail in model.PurchaseOrderDetailItems)
                    {
                        detail.PurchaseOrderId = purchaseOrder.Id;
                        await _purchaseOrderDetailService.AddAsync(detail);
                    }
                    return RedirectToAction("Index", "Purchaser");
                }
                ModelState.AddModelError("", "Error adding purchase order.");
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

        public async Task<IActionResult> OrderDetails(Guid OrderId)
        {
            var orderDetails =  await _purchaseOrderService.OrderDetails(OrderId);

            var model = new PurchaseOrderDetailsVM
            {
                Id = orderDetails.Id,
                POCode = orderDetails.POCode,
                PurchaserName = orderDetails.Purchaser.FullName, 
                PurchaserEmail = orderDetails.Purchaser.Email, 
                SupplierName = orderDetails.Supplier.FullName,   
                SupplierEmail = orderDetails.Supplier.Email,
                DeliveryDate = orderDetails.DeliveryDate,
                Status = orderDetails.Status,
                Notes = orderDetails.Notes,
                TotalCost = orderDetails.TotalCost,
                PurchaseOrderItems = orderDetails.PurchaseOrderDetails.Select(detail => new PurchaseOrderItemVM
                {
                    ProductName = detail.Product.Name,
                    PurchasePrice = detail.PurchasePrice,
                    Quantity = detail.Quantity
                }).ToList()
            };
            return View(model);
        }


        public async Task<IActionResult> GenerateInvoicePdf(Guid OrderId)
        {
            var orderDetails = await _purchaseOrderService.OrderDetails(OrderId);

            var model = new PurchaseOrderDetailsVM
            {
                POCode = orderDetails.POCode,
                PurchaserName = orderDetails.Purchaser.FullName,
                PurchaserEmail = orderDetails.Purchaser.Email,
                SupplierName = orderDetails.Supplier.FullName,
                SupplierEmail = orderDetails.Supplier.Email,
                DeliveryDate = orderDetails.DeliveryDate,
                Status = orderDetails.Status,
                Notes = orderDetails.Notes,
                TotalCost = orderDetails.TotalCost,
                PurchaseOrderItems = orderDetails.PurchaseOrderDetails.Select(detail => new PurchaseOrderItemVM
                {
                    ProductName = detail.Product.Name,
                    PurchasePrice = detail.PurchasePrice,
                    Quantity = detail.Quantity
                }).ToList()
            };
            return View(model);
        }

    }
}
