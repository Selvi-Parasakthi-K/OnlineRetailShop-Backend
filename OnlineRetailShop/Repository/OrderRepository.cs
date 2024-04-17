using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using OnlineRetailShop.Cache;
using OnlineRetailShop.Models;
using OnlineRetailShop.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailShop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;
        private readonly CommonContext _dbContext;
        private readonly IMemoryCache _cache;
        //public readonly string cachekey = "mostsoldproduct";

        

        public OrderRepository(ILogger<OrderRepository> logger,CommonContext dbContext, IMemoryCache cache)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _dbContext.Order.ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            return await _dbContext.Order.FindAsync(id);
        }

        public async Task<Order> CreateOrder(CreateOrder items)
        {
            Order order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = items.CustomerId,
                ProductId = items.ProductId,
                Quantity = items.Quantity,
                IsCancel = true
            };

            _dbContext.Order.Add(order);

            var product = await _dbContext.Product.FindAsync(items.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            if (product.quantity < items.Quantity)
            {
                throw new ArgumentException("No sufficient amount of product");
            }

            product.quantity -= items.Quantity;

            await _dbContext.SaveChangesAsync();

            _cache.Remove("mostsoldproduct");

            return order;
        }

        public async Task<bool> UpdateOrder(Guid id, Order order)
        {
            if (id != order.OrderId)
            {
                return false;
            }

            _dbContext.Entry(order).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderAvailable(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            var order = await _dbContext.Order.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _dbContext.Order.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private bool OrderAvailable(Guid id)
        {
            return _dbContext.Order.Any(x => x.OrderId == id);
        }

        //public async Task<Product> GetMostSoldProduct()
        //{
        //    var productSales = await _dbContext.Order
        //        .GroupBy(pi => pi.ProductId)
        //       .Select(s => new
        //        {
        //            ProductId = s.Key,
        //            TotalSales = s.Sum(q => q.Quantity)
        //        }).ToListAsync();
        //    var maxSales = productSales.Max(ps => ps.TotalSales);
        //    var mostSoldProductId = productSales.First(ps => ps.TotalSales == maxSales).ProductId;

        //    var mostSoldProduct = await _dbContext.Product.FirstOrDefaultAsync(p => p.Id == mostSoldProductId);
        //    return mostSoldProduct;
        //}


        public async Task<Product> Index()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if(_cache.TryGetValue("mostsoldproduct", out Product mostSoldProduct))
            {
                _logger.LogInformation("Product found");
                return mostSoldProduct;
          }

                _logger.LogInformation("Product found");

                var productsales = await _dbContext.Order
                    .GroupBy(pi => pi.ProductId)
                    .Select(s => new
                    {
                        productid = s.Key,
                        totalsales = s.Sum(q => q.Quantity)
                    }).ToListAsync();
                var maxsales = productsales.Max(ps => ps.totalsales);
                var mostsoldproductid = productsales.First(ps => ps.totalsales == maxsales).productid;

                mostSoldProduct = await _dbContext.Product.FirstOrDefaultAsync(p => p.Id == mostsoldproductid);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal);

                _cache.Set("mostsoldproduct", mostSoldProduct, cacheEntryOptions);

           
            stopwatch.Stop();

            _logger.LogInformation($"Passed time : {stopwatch.ElapsedMilliseconds} ms ");

            return mostSoldProduct;
        }

        public void ClearCache()
        {
           _cache.Remove("mostsoldproduct");
            _logger.Log(LogLevel.Information, "Cleared cache");
        }
    }
}

