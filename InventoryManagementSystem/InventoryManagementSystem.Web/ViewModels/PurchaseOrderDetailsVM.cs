using InventoryManagementSystem.Data.Enums;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class PurchaseOrderDetailsVM
    {
        public Guid Id { get; set; }
        public string POCode { get; set; }
        public string PurchaserName { get; set; }
        public string PurchaserEmail { get; set; }
        public string SupplierName { get; set; }
        public string SupplierEmail { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public double TotalCost { get; set; }

        public List<PurchaseOrderItemVM> PurchaseOrderItems { get; set; }
    }

    public class PurchaseOrderItemVM
    {
        public string ProductName { get; set; }
        public double PurchasePrice { get; set; }
        public int Quantity { get; set; }
        public double TotalCost => PurchasePrice * Quantity;
    }

}
