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
    [Authorize(Roles = UserRoles.Purchaser)]
    public class PurchaserController : Controller
    {

        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderDetailService _purchaseOrderDetailService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IPurchaserService _purchaserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PurchaserController(
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Purchaser purchaser = await _purchaserService.GetByIdAsync(u => u.UserId == userId);

            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.PurchaserId == purchaser.Id,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }

        public async Task<IActionResult> PendingPurchases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Purchaser purchaser = await _purchaserService.GetByIdAsync(u => u.UserId == userId);

            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.PurchaserId == purchaser.Id && u.Status == OrderStatus.Pending,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }

        public async Task<IActionResult> VerifiedPurchases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Purchaser purchaser = await _purchaserService.GetByIdAsync(u => u.UserId == userId);

            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.PurchaserId == purchaser.Id && u.Status == OrderStatus.Verified,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }
        public async Task<IActionResult> CanceiledPurchases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Purchaser purchaser = await _purchaserService.GetByIdAsync(u => u.UserId == userId);

            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.PurchaserId == purchaser.Id && u.Status == OrderStatus.Canceiled,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }
        public async Task<IActionResult> DeliveredPurchases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Purchaser purchaser = await _purchaserService.GetByIdAsync(u => u.UserId == userId);

            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.PurchaserId == purchaser.Id && u.Status == OrderStatus.Delivered,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();

            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyPO(Guid id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(u => u.Id == id, includeProperties: "PurchaseOrderDetails,PurchaseOrderDetails.Product");

            if (purchaseOrder == null)
            {
                return NotFound("Purchase order not found.");
            }

            purchaseOrder.Status = OrderStatus.Verified;

            foreach (var item in purchaseOrder.PurchaseOrderDetails)
            {
                var product = await _productService.GetByIdAsync(u => u.Id == item.ProductId);

                if (product != null)
                {
                     product.StockLevel += item.Quantity;
                    await _productService.UpdateAsync(product);
                }
            }
            
            purchaseOrder.DeliveryDate = DateTime.Now;
            var success = await _purchaseOrderService.UpdateAsync(purchaseOrder);

            if (success)
            {
                TempData["success"] = "The Purchase order verified.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Verification Error.";


            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> CanceilPO(Guid id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(u => u.Id == id, includeProperties: "PurchaseOrderDetails,PurchaseOrderDetails.Product");

            if (purchaseOrder == null)
            {
                return NotFound("Purchase order not found.");
            }

            purchaseOrder.Status = OrderStatus.Canceiled;

            var success = await _purchaseOrderService.UpdateAsync(purchaseOrder);

            if (success)
            {
                TempData["success"] = "The Purchase order canceiled.";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Cancelation Error.";


            return RedirectToAction(nameof(Index));
        }
    }
}
