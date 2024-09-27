using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class PurchaseOrderVM
    {
        public string CurrentPurchaserName { get; set; }

        public DateTime CurrentDate = DateTime.Now;

        public string nextPOCode { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }

        [ValidateNever]
        public IEnumerable<PurchaseOrder> PurchaseOrderList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SuppliersList { get; set; }
        public List<PurchaseOrderDetailItem> PurchaseOrderDetailItems { get; set; }

        public class PurchaseOrderDetailItem : PurchaseOrderDetail
        {

        }
    }
}
