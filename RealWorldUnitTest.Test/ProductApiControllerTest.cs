using Microsoft.AspNetCore.Mvc;
using Moq;
using RealWorldUnitTest.Web.Controllers;
using RealWorldUnitTest.Web.Helpers;
using RealWorldUnitTest.Web.Models;
using RealWorldUnitTest.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RealWorldUnitTest.Test
{
    public class ProductApiControllerTest
    {
        private readonly Mock<IRepository<Product>> _mockRepo;
        private readonly ProductsApiController _controller;
        private List<Product> products;
        private readonly Helper _helper;

        public ProductApiControllerTest(Mock<IRepository<Product>> mockRepo, ProductsApiController controller, List<Product> products, Helper helper)
        {
            _mockRepo = mockRepo;
            _controller = controller;
            this.products = products;
            _helper = helper;
        }

        [Theory]
        [InlineData(4, 5, 9)]
        public void Add_SampleBalues_ReturnTotal(int a, int b, int total)
        {
            var result = _helper.add(a, b);
            Assert.Equal(total, result);
        }

        [Fact]
        public async void GetProduct_ActionExecutes_RetrunOkResultWithProduct()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);
            var result = await _controller.GetProduct();
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnProduct = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(2, returnProduct.ToList().Count);
        }

        [Theory]
        [InlineData(0)]
        public async void GetProduct_IdIsNull_ReturnNotFound(int productId)
        {
            Product product = null;

            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _controller.GetProduct(productId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetProduct_IdValid_ReturnOkResult(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var result = await _controller.GetProduct(productId);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnProduct.Id);
            Assert.Equal(product.Name, returnProduct.Name);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void PutProduct_IdIsNotEqualProduct_ReturnBadRequestResult(int productId)
        {
            var product = products.First(x => x.Id == productId);
            var result = _controller.PutProduct(2, product);
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public void PutProduct_ActionExecutes_ReturnNoContent(int productId)
        {
            var product = products.First(x => x.Id == productId);
            _mockRepo.Setup(x => x.Update(product));
            var result = _controller.PutProduct(productId, product);
            _mockRepo.Verify(x => x.Update(product), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void PostProduct_ActionExecutes_ReturnCreatedAction()
        {
            var product = products.First();
            _mockRepo.Setup(x => x.Create(product)).Returns(Task.CompletedTask);
            var result = await _controller.PostProduct(product);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            _mockRepo.Verify(x => x.Create(product), Times.Once);
            Assert.Equal("GetProduct", createdAtActionResult.ActionName);
        }



        [Theory]
        [InlineData(1)]
        public async void DeleteProduct_IdInValid_ReturnNotFound(int productId)
        {
            Product product = null;
            _mockRepo.Setup(x => x.GetById(productId)).ReturnsAsync(product);
            var resultNotFound = await _controller.DeleteProduct(productId);
            Assert.IsType<NotFoundResult>(resultNotFound);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteProduct_ActionExecute_ReturnNoContent(int prodcutId)
        {
            var product = products.First(x => x.Id == prodcutId);
            _mockRepo.Setup(x => x.GetById(prodcutId)).ReturnsAsync(product);
            _mockRepo.Setup(x => x.Delete(product));
            var noContentResult = await _controller.DeleteProduct(prodcutId);
            _mockRepo.Verify(x => x.Delete(product), Times.Once);
            Assert.IsType<NoContentResult>(noContentResult);
        }

    }
}