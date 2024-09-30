using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Contracts
{
    public interface IFinancialReportService
    {
        Task<double> GetTotalRevenueAsync();
        Task<double> GetTotalCOGSAsync();
        Task<double> GetTotalProfitAsync();
        Task<double> GetTotalLossAsync();
    }

}
