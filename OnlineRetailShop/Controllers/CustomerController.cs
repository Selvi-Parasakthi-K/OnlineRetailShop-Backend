using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineRetailShop.Filter;
using OnlineRetailShop.Models;
using OnlineRetailShop.Repository;

namespace OnlineRetailShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [Authorize]
        [HttpGet]
        [Route("GetAllCustomer")]
        [ServiceFilter(typeof(AuthorizationFilter))]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllCustomers();
            return Ok(customers);
        }

        [HttpGet("GetCustomerById/{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            var customer = await _customerRepository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var createdCustomer = await _customerRepository.CreateCustomer(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.customerId }, createdCustomer);
        }

        [HttpPut("EditCustomer/{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            if (id != customer.customerId)
            {
                return BadRequest("Id mismatch");
            }

            try
            {
                await _customerRepository.UpdateCustomer(id, customer);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            try
            {
                await _customerRepository.DeleteCustomer(id);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }
    }
}
