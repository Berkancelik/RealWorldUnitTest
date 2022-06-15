﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using RealWorldUnitTest.Web.Controllers;
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

        public ProductApiControllerTest()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _controller = new ProductsApiController(_mockRepo.Object);
            products = new List<Product>() { new Product { Id = 1, Name = "Kalem", Price = 500, Stock = 54, Color = "Mavi" },
                new Product { Id = 2, Name = "Defter", Price = 450, Stock = 22, Color = "Turuncu" } };
        }
        [Fact]
        public async void GetProduct_ActionExecutes_RetrunOkResultWithProduct()
        {
            _mockRepo.Setup(x => x.GetAll()).ReturnsAsync(products);
            var result = await _controller.GetProduct();
            var okResult = Assert.IsType<ObjectResult>(result);
            var returnProduct = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal<int>(1, returnProduct.ToList().Count);
        }
    }
}
