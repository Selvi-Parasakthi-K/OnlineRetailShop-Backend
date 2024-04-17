using Microsoft.AspNetCore.Mvc;
using OnlineRetailShop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRetailShop.Repository
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(Guid id);
        Task<Order> CreateOrder(CreateOrder items);
        Task<bool> UpdateOrder(Guid id, Order order);
        Task<bool> DeleteOrder(Guid id);

        //Task<Product> GetMostSoldProduct();

        //Task<Product> GetMostSoldProduct();

        //public IActionResult Index();
        void ClearCache();

       Task<Product> Index();
    }
    
}

