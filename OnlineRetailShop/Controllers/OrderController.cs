using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailShop.Models;
using OnlineRetailShop.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineRetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return Ok(orders);
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrder items)
        {
            try
            {
                var order = await _orderRepository.CreateOrder(items);
                return CreatedAtAction(nameof(GetOrders), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("EditOrder")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            try
            {
                bool updated = await _orderRepository.UpdateOrder(id, order);
                if (!updated)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                bool deleted = await _orderRepository.DeleteOrder(id);
                if (!deleted)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetMostSoldProduct")]
        public async Task<ActionResult<Product>> GetMostSoldProduct()
        { 
            var mostSoldProduct = await _orderRepository.Index();
            if(mostSoldProduct == null)
            {
                return NotFound("No product sold yet");
            }
            return Ok(mostSoldProduct);
        }
    }
}
