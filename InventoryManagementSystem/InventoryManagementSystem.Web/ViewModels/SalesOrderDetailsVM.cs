using InventoryManagementSystem.Data.Enums;

namespace InventoryManagementSystem.Web.ViewModels
{
    public class SalesOrderDetailsVM
    {
        public Guid Id { get; set; }
        public string SOCode { get; set; }
        public string SalesmanName { get; set; }
        public string SalesmanEmail { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerEmail { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public double TotalCost { get; set; }

        public List<SalesOrderItemVM> SalesOrderItems { get; set; }
    }

    public class SalesOrderItemVM
    {
        public string ProductName { get; set; }
        public double SalesPrice { get; set; }
        public int Quantity { get; set; }
        public double TotalCost => SalesPrice * Quantity;
    }

}
