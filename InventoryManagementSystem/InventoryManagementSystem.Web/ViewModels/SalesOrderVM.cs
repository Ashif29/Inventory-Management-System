using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Data.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class SalesOrderVM
    {
        public string CurrentSalesmanName { get; set; }

        public DateTime CurrentDate = DateTime.Now;

        public string nextSOCode { get; set; }
        public SalesOrder SalesOrder { get; set; }

        [ValidateNever]
        public IEnumerable<SalesOrder> SalesOrderList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ConsumersList { get; set; }
        public List<SalesOrderDetailItem> SalesOrderDetailItems { get; set; }

        public class SalesOrderDetailItem : SalesOrderDetail
        {

        }
    }
}
