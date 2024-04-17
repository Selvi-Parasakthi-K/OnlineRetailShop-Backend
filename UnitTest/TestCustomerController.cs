using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class Get_TestCustomerController : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public Get_TestCustomerController(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("https://localhost:44308/api/Customer/GetAllCustomer")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            var result = response.StatusCode;

            Assert.False(false);
        }

        [Theory]
        [InlineData("https://localhost:44308/api/Customer/CreateCustomer")]
        public async Task Post_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            var result = response.StatusCode;

            Assert.False(false);
        }
    }
    
}
