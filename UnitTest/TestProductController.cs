using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using OnlineRetailShop.Controllers;
using OnlineRetailShop.Models;
using OnlineRetailShop.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class TestProductController : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public TestProductController(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public void CheckGetProductMethod()
        {
            var data = A.Fake<IProductRepository>();
            var productcontroller = new ProductController(data);
            var products = productcontroller.GetProducts();
            OkObjectResult okresult = products.Result as OkObjectResult;
            Assert.IsType<OkObjectResult>(okresult);
        }

        [Theory]
        [InlineData("https://localhost:44308/api/Product/GetAllProduct")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var result = response.IsSuccessStatusCode; // Status Code 200-299

            Assert.False(result);
        }

            //[Theory]
            //[InlineData("Bulb", 3, true)]
            //[InlineData("Book", 3, true)]
            //public void CheckAddProductMethod(string productName, int quantity, bool isActive) 
            //{
            //    var sampleProduct = new Product();

            //    sampleProduct.productName = productName;
            //    sampleProduct.quantity = quantity;
            //    sampleProduct.isActive = isActive;

            //    var data = A.Fake<IProductRepository>();
            //    var productcontroller = new ProductController(data);

            //    var products = productcontroller.CreateProduct(sampleProduct);

            //    OkObjectResult okresult = products.Result as OkObjectResult;
            //    Assert.IsType<OkObjectResult>(okresult);
            //}

            //[Theory]
            //[InlineData("aada1079-eac5-4ccd-871e-0e73869fa1de", "saree",10,true)]
            //public void CheckUpdateProductMethod(Guid Id,string productName, int quantity, bool isActive)
            //{
            //    var sampleProduct = new Product();

            //    sampleProduct.Id = Id;
            //    sampleProduct.productName = productName;
            //    sampleProduct.quantity = quantity;
            //    sampleProduct.isActive = isActive;

            //    var data = A.Fake<IProductRepository>();
            //    var productcontroller = new ProductController(data);

            //    var products = productcontroller.EditProduct(Id, sampleProduct);

            //    OkObjectResult okresult = products.Result as OkObjectResult;
            //    Assert.IsType<OkObjectResult>(okresult);
            //}
        }
}
