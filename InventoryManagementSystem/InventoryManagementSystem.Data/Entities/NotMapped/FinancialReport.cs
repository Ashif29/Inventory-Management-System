using InventoryManagementSystem.Data.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Entities.NotMapped
{
    public class FinancialReport : BaseEntity
    {
        public double? TotalRevenue { get; set; }
        public double? TotalCOGS { get; set; }
        public double? TotalProfit => (TotalRevenue ?? 0) - (TotalCOGS ?? 0);
        public double? TotalLoss => TotalProfit < 0 ? Math.Abs(TotalProfit.Value) : 0;
    }

}
