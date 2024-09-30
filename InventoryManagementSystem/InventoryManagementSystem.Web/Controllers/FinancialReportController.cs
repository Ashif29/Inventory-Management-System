using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers
{
    public class FinancialReportController : Controller
    {
        private readonly IFinancialReportService _financialReportService;

        public FinancialReportController(IFinancialReportService financialReportService)
        {
            _financialReportService = financialReportService;
        }


        
        public async Task<IActionResult> TotalCOGS()
        {
            double totalCOGS = await _financialReportService.GetTotalCOGSAsync();
            return View(totalCOGS);
        }

        public async Task<IActionResult> TotalRevenue()
        {
            double totalRevenue = await _financialReportService.GetTotalRevenueAsync();
            return View(totalRevenue);
        }

        public async Task<IActionResult> TotalProfit()
        {
            double totalProfit = await _financialReportService.GetTotalProfitAsync();
            return View(totalProfit);
        }

        public async Task<IActionResult> TotalLoss()
        {
            double totalLoss = await _financialReportService.GetTotalLossAsync();
            return View(totalLoss);
        }
    }
}

