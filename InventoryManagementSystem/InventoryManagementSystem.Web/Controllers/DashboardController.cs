using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using InventoryManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class DashboardController : Controller
    {
        private readonly IFinancialReportService _financialReportService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IFinancialReportService financialReportService, ILogger<DashboardController> logger)
        {
            _financialReportService = financialReportService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching financial metrics for the dashboard");

            double totalCOGS = await _financialReportService.GetTotalCOGSAsync();
            _logger.LogInformation("Total COGS retrieved: {TotalCOGS}", totalCOGS);

            double totalRevenue = await _financialReportService.GetTotalRevenueAsync();
            _logger.LogInformation("Total Revenue retrieved: {TotalRevenue}", totalRevenue);

            double totalProfit = await _financialReportService.GetTotalProfitAsync();
            _logger.LogInformation("Total Profit retrieved: {TotalProfit}", totalProfit);

            double totalLoss = await _financialReportService.GetTotalLossAsync();
            _logger.LogInformation("Total Loss retrieved: {TotalLoss}", totalLoss);

            var financialReportVM = new FinancialReportVM
            {
                TotalCOGS = totalCOGS,
                TotalProfit = totalProfit,
                TotalRevenue = totalRevenue,
                TotalLoss = totalLoss
            };

            return View(financialReportVM);
        }
    }

}
