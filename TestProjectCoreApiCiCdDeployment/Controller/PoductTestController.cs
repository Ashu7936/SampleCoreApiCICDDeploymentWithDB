using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleCoreApiCICDDeploymentWithDB.Controllers;
using SampleCoreApiCICDDeploymentWithDB.DataAccess;
using SampleCoreApiCICDDeploymentWithDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCoreApiCiCdDeployment.Controller
{
    public class PoductTestController
    {
        private MyAppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MyAppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new MyAppDbContext(options);
        }

        // ---------------- GET ALL ----------------

        [Fact]
        public void Get_Returns_NotFound_When_No_Products()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new ProductController(context);

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_Returns_Ok_When_Products_Exist()
        {
            // Arrange
            var context = GetDbContext();
            context.Products.Add(new Product { Name = "Pen", Price = 10, Qty = 5 });
            context.SaveChanges();

            var controller = new ProductController(context);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Single(products);
        }

        // ---------------- GET BY ID ----------------

        [Fact]
        public void GetById_Returns_NotFound_When_Product_Not_Exists()
        {
            var context = GetDbContext();
            var controller = new ProductController(context);

            var result = controller.Get(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetById_Returns_Product_When_Found()
        {
            var context = GetDbContext();
            context.Products.Add(new Product { Id = 1, Name = "Book", Price = 100, Qty = 2 });
            context.SaveChanges();

            var controller = new ProductController(context);

            var result = controller.Get(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Book", product.Name);
        }

        // ---------------- POST ----------------

        [Fact]
        public void Post_Creates_New_Product()
        {
            var context = GetDbContext();
            var controller = new ProductController(context);

            var product = new Product
            {
                Name = "Laptop",
                Price = 50000,
                Qty = 1
            };

            var result = controller.Post(product);

            Assert.IsType<OkObjectResult>(result);
            Assert.Single(context.Products);
        }

        // ---------------- PUT ----------------

        [Fact]
        public void Put_Returns_BadRequest_When_Id_Invalid()
        {
            var context = GetDbContext();
            var controller = new ProductController(context);

            var result = controller.Put(new Product { Id = 0 });

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Put_Updates_Product_When_Valid()
        {
            var context = GetDbContext();
            context.Products.Add(new Product { Id = 1, Name = "Mouse", Price = 500, Qty = 2 });
            context.SaveChanges();

            var controller = new ProductController(context);

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Gaming Mouse",
                Price = 1500,
                Qty = 3
            };

            var result = controller.Put(updatedProduct);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Gaming Mouse", context.Products.First().Name);
        }

        // ---------------- DELETE ----------------

        [Fact]
        public void Delete_Returns_BadRequest_When_Product_Not_Found()
        {
            var context = GetDbContext();
            var controller = new ProductController(context);

            var result = controller.Delete(1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Delete_Removes_Product_When_Found()
        {
            var context = GetDbContext();
            context.Products.Add(new Product { Id = 1, Name = "Tablet", Price = 20000, Qty = 1 });
            context.SaveChanges();

            var controller = new ProductController(context);

            var result = controller.Delete(1);

            Assert.IsType<OkObjectResult>(result);
            Assert.Empty(context.Products);
        }

    }
}
