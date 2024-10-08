﻿using InventoryManagementSystem.Data.Entities;
using InventoryManagementSystem.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Data.Repositories.Core
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IPurchaserRepository PurchaserRepository { get; }
        IPurchaseOrderRepository PurchaseOrderRepository { get; }
        IPurchaseOrderDetailRepository PurchaseOrderDetailRepository { get; }
        ISalesOrderDetailRepository SalesOrderDetailRepository { get; }
        IConsumerRepository ConsumerRepository { get; }
        ISalesmanRepository SalesmanRepository { get; }
        ISalesOrderRepository SalesOrderRepository { get; }


        Task<bool> CompleteAsync();
    }
}
