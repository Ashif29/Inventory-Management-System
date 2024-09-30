using InventoryManagementSystem.Data.DataAccess;
using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Enums;
using InventoryManagementSystem.Data.Repositories.Core;
using InventoryManagementSystem.Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Service.Services.Implementations
{
    public class FinancialReportService : IFinancialReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FinancialReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<double> GetTotalCOGSAsync()
        {
            var verifiedSalesOrderDetails = _unitOfWork.SalesOrderRepository.GetVerifiedSalesOrderDetails();

            var verifiedPurchaseOrderDetails = _unitOfWork.PurchaseOrderRepository.GetVerifiedPurchaseOrderDetails();

            double totalCOGS = 0;

            foreach (var sod in verifiedSalesOrderDetails)
            {
                var matchingPurchaseOrders = verifiedPurchaseOrderDetails
                    .Where(pod => pod.ProductId == sod.ProductId)
                    .OrderBy(pod => pod.PurchasePrice)
                    .ToList();

                int quantityToBeSold = sod.Quantity;

                foreach (var pod in matchingPurchaseOrders)
                {
                    if (quantityToBeSold == 0) break;

                    if (pod.Quantity >= quantityToBeSold)
                    {
                        totalCOGS += quantityToBeSold * pod.PurchasePrice;
                        quantityToBeSold = 0;
                    }
                    else
                    {
                        totalCOGS += pod.Quantity * pod.PurchasePrice;
                        quantityToBeSold -= pod.Quantity;
                    }
                }
            }

            return totalCOGS;
        }
        public async Task<double> GetTotalRevenueAsync()
        {
            var verifiedSalesOrders = _unitOfWork.SalesOrderRepository.GetVerifiedSalesOrderDetails();
            return await verifiedSalesOrders
                .SumAsync(sod => (double)(sod.SalesPrice * sod.Quantity));
        }

        public async Task<double> GetTotalProfitAsync()
        {
            double totalRevenue = await GetTotalRevenueAsync();
            double totalCOGS = await GetTotalCOGSAsync();
            double totalProfit  = totalRevenue - totalCOGS;
            return totalProfit < 0 ? 0 : totalCOGS;
        }

        public async Task<double> GetTotalLossAsync()
        {
            double totalRevenue = await GetTotalRevenueAsync();
            double totalCOGS = await GetTotalCOGSAsync();
            double totalLoss = totalCOGS - totalRevenue;
            return totalLoss < 0 ? 0 : totalLoss;
        }
    }



}
