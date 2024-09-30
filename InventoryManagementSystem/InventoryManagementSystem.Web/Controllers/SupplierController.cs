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
    [Authorize(Roles = UserRoles.Supplier)]
    public class SupplierController : Controller
    {

        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderDetailService _purchaseOrderDetailService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IPurchaserService _purchaserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(
            IPurchaseOrderService purchaseOrderService,
            IPurchaseOrderDetailService purchaseOrderDetailService,
            IProductService productService,
            ISupplierService supplierService,
            IPurchaserService purchaserService,
            UserManager<ApplicationUser> userManager,
            ILogger<SupplierController> logger)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseOrderDetailService = purchaseOrderDetailService;
            _productService = productService;
            _supplierService = supplierService;
            _purchaserService = purchaserService;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Supplier supplier = await _supplierService.GetByIdAsync(u => u.UserId == userId);

            _logger.LogInformation("Fetching all purchase orders for supplier with ID: {SupplierId}", supplier.Id);


            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.SupplierId == supplier.Id,
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
            Supplier supplier = await _supplierService.GetByIdAsync(u => u.UserId == userId);


            _logger.LogInformation("Fetching pending purchase orders for supplier with ID: {SupplierId}", supplier.Id);


            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.SupplierId == supplier.Id && u.Status == OrderStatus.Pending,
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
            Supplier supplier = await _supplierService.GetByIdAsync(u => u.UserId == userId);


            _logger.LogInformation("Fetching verified purchase orders for supplier with ID: {SupplierId}", supplier.Id);


            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.SupplierId == supplier.Id && u.Status == OrderStatus.Verified,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);

            return View(model);
        }

        public async Task<IActionResult> CanceledPurchases()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Supplier supplier = await _supplierService.GetByIdAsync(u => u.UserId == userId);


            _logger.LogInformation("Fetching canceled purchase orders for supplier with ID: {SupplierId}", supplier.Id);


            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.SupplierId == supplier.Id && u.Status == OrderStatus.Canceled,
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
            Supplier supplier = await _supplierService.GetByIdAsync(u => u.UserId == userId);


            _logger.LogInformation("Fetching delivered purchase orders for supplier with ID: {SupplierId}", supplier.Id);


            var purchaseOrders = (await _purchaseOrderService.GetAllAsync(
                                u => u.SupplierId == supplier.Id && u.Status == OrderStatus.Delivered,
                                includeProperties: "Purchaser,Supplier"))
                                .OrderByDescending(u => u.CreatedAt)
                                .ToList();
            var model = new PurchaseOrderVM();
            model.PurchaseOrderList = purchaseOrders;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CancelPO(Guid id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(u => u.Id == id);

            if (purchaseOrder == null)
            {
                _logger.LogWarning("Attempted to cancel a purchase order that does not exist: {OrderId}", id);

                return NotFound("Purchase order not found.");
            }

            purchaseOrder.Status = OrderStatus.Canceled;

            var success = await _purchaseOrderService.UpdateAsync(purchaseOrder);

            if (success)
            {
                TempData["success"] = "The Purchase order canceled.";
                _logger.LogInformation("Purchase order {OrderId} has been canceled successfully.", id);

                return RedirectToAction("Index");
            }
            TempData["error"] = "Cancelation Error.";


            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> DeliverPO(Guid id)
        {
            var purchaseOrder = await _purchaseOrderService.GetByIdAsync(u => u.Id == id);

            if (purchaseOrder == null)
            {
                _logger.LogWarning("Attempted to deliver a purchase order that does not exist: {OrderId}", id);

                return NotFound("Purchase order not found.");
            }

            purchaseOrder.Status = OrderStatus.Delivered;

            var success = await _purchaseOrderService.UpdateAsync(purchaseOrder);

            if (success)
            {
                TempData["success"] = "The Purchase order delivered.";
                _logger.LogInformation("Purchase order {OrderId} has been delivered successfully.", id);

                return RedirectToAction("Index");
            }
            TempData["error"] = "Delivery Error.";
            _logger.LogError("Failed to deliver purchase order {OrderId}.", id);

            return RedirectToAction(nameof(Index));
        }
    }
}
