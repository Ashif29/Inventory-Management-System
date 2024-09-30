using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly IFinancialReportService _financialReportService;

        public DashboardController(IFinancialReportService financialReportService)
        {
            _financialReportService = financialReportService;
        }
        public async Task<IActionResult> Index()
        {
            double totalCOGS = await _financialReportService.GetTotalCOGSAsync();
            double totalRevenue = await _financialReportService.GetTotalRevenueAsync();
            double totalProfit = await _financialReportService.GetTotalProfitAsync();
            double totalLoss = await _financialReportService.GetTotalLossAsync();

            var financialReportVM = new FinancialReportVM();

            financialReportVM.TotalCOGS = totalCOGS;
            financialReportVM.TotalProfit = totalProfit;
            financialReportVM.TotalRevenue = totalRevenue;
            financialReportVM.TotalLoss = totalLoss;
            return View(financialReportVM);
        }
    }
}
