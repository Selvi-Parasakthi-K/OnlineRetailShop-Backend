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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return Ok(products);
        }

        [HttpPost]
        [Route("CreateProduct")] 
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                var createdProduct = await _productRepository.CreateProduct(product);
                return Ok(createdProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct(Guid id, Product product)
        {
            try
            {
                bool updated = await _productRepository.UpdateProduct(id, product);
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
        [Route("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                bool deleted = await _productRepository.DeleteProduct(id);
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
    }
}
