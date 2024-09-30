using InventoryManagementSystem.Data.Entities.NotMapped;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Web.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class FinancialReportController : Controller
    {
        private readonly IFinancialReportService _financialReportService;
        private readonly ILogger<FinancialReportController> _logger;

        public FinancialReportController(IFinancialReportService financialReportService, ILogger<FinancialReportController> logger)
        {
            _financialReportService = financialReportService;
            _logger = logger;
        }

        public async Task<IActionResult> TotalCOGS()
        {
            _logger.LogInformation("Fetching total COGS");
            double totalCOGS = await _financialReportService.GetTotalCOGSAsync();
            _logger.LogInformation("Total COGS retrieved: {TotalCOGS}", totalCOGS);
            return View(totalCOGS);
        }

        public async Task<IActionResult> TotalRevenue()
        {
            _logger.LogInformation("Fetching total revenue");
            double totalRevenue = await _financialReportService.GetTotalRevenueAsync();
            _logger.LogInformation("Total revenue retrieved: {TotalRevenue}", totalRevenue);
            return View(totalRevenue);
        }

        public async Task<IActionResult> TotalProfit()
        {
            _logger.LogInformation("Fetching total profit");
            double totalProfit = await _financialReportService.GetTotalProfitAsync();
            _logger.LogInformation("Total profit retrieved: {TotalProfit}", totalProfit);
            return View(totalProfit);
        }

        public async Task<IActionResult> TotalLoss()
        {
            _logger.LogInformation("Fetching total loss");
            double totalLoss = await _financialReportService.GetTotalLossAsync();
            _logger.LogInformation("Total loss retrieved: {TotalLoss}", totalLoss);
            return View(totalLoss);
        }
    }

}

