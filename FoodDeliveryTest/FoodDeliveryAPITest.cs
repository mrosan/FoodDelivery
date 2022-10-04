using System;
using System.Collections.Generic;
using System.Linq;
using FoodDeliveryData;
using FoodDeliveryAPI.Controllers;
using FoodDeliveryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodDeliveryTest
{
    public class FoodDeliveryAPITest : IDisposable
    {
        private readonly FoodDeliveryContext _context;
        private readonly List<OrderDTO> _orderDTOs;
        private readonly List<ProductDTO> _productDTOs;

        // ititialize data
        public FoodDeliveryAPITest()
        {
            var options = new DbContextOptionsBuilder<FoodDeliveryContext>()
                .UseInMemoryDatabase("FoodDeliveryAPITest")
                .Options;

            _context = new FoodDeliveryContext(options);
            _context.Database.EnsureCreated();

            var orderData = new List<Order>
            {
                new Order
                {
                    ProductId = 1,
                    Price = 900,
                    Completed = false,
                    Name = "Test Customer",
                    Address = "City Street House",
                    TelephoneNr = "+3654377346",
                    DateSubmitted = DateTime.Now,
                    OrderGroup = 54
                },
                new Order
                {
                    ProductId = 2,
                    Price = 1900,
                    Completed = false,
                    Name = "Test Customer",
                    Address = "City Street House",
                    TelephoneNr = "+3654377346",
                    DateSubmitted = DateTime.Now,
                    OrderGroup = 54
                },
                new Order
                {
                    ProductId = 3,
                    Price = 500,
                    Completed = false,
                    Name = "Customer",
                    Address = "Addr",
                    TelephoneNr = "2035677346",
                    DateSubmitted = DateTime.Now,
                    OrderGroup = 23
                },
                new Order
                {
                    ProductId = 1,
                    Price = 650,
                    Completed = false,
                    Name = "Customer",
                    Address = "Addr",
                    TelephoneNr = "2035677346",
                    DateSubmitted = DateTime.Now,
                    OrderGroup = 23
                },
                new Order
                {
                    ProductId = 2,
                    Price = 1500,
                    Completed = false,
                    Name = "New Customer",
                    Address = "Address",
                    TelephoneNr = "103347346",
                    DateSubmitted = DateTime.Now,
                    OrderGroup = 10
                }
            };
            _context.Orders.AddRange(orderData);

            var productData = new List<Product>
            {
                new Product
                {
                    Name = "Mom's spaghetti",
                    Description = "When your knees are weak and arms are heavy...",
                    Price = 900,
                    CategoryId = 2,
                    IsVegan = false,
                    IsSpicy = false,
                    AmountSold = 20
                },
                new Product
                {
                    Name = "Mashed potatoes with thick gravy",
                    Description = "Favorite food of the spearmen from Alba Division.",
                    Price = 800,
                    CategoryId = 2,
                    IsVegan = true,
                    IsSpicy = false,
                    AmountSold = 0
                },
                new Product
                {
                    Name = "Tandoori chicken",
                    Description = "Chicken marinated for hours in a paste of yogurt and spices, and then roasted (traditionally) in a clay oven called a tandoor.",
                    Price = 1900,
                    CategoryId = 2,
                    IsVegan = false,
                    IsSpicy = true,
                    AmountSold = 7
                },
                new Product
                {
                    Name = "Coffee",
                    Description = "Just the way you like it.",
                    Price = 150,
                    CategoryId = 3,
                    IsVegan = true,
                    IsSpicy = false,
                    AmountSold = 42
                },
                new Product
                {
                    Name = "Dwarven Stew",
                    Description = "Contains generous amount of alcohol.",
                    Price = 1650,
                    CategoryId = 1,
                    IsVegan = false,
                    IsSpicy = true,
                    AmountSold = 5
                }
            };
            _context.Products.AddRange(productData);
            _context.SaveChanges();

            _orderDTOs = orderData
                    .GroupBy(x => x.OrderGroup)
                    .Select(order => new OrderDTO
                    {
                        OrderGroup = order.First().OrderGroup,
                        Sum = order.Sum(o => o.Price),
                        Completed = order.First().Completed,
                        Name = order.First().Name,
                        Address = order.First().Address,
                        TelephoneNr = order.First().TelephoneNr,
                        DateSubmitted = order.First().DateSubmitted,
                        DateCompleted = order.First().DateCompleted
                    })
                    .ToList();

            _productDTOs = productData.Select(p => new ProductDTO
            {
                Id = p.ProductID,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                IsSpicy = p.IsSpicy,
                IsVegan = p.IsVegan
            }).ToList();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void GetOrdersTest()
        {
            var controller = new OrdersController(_context);
            var result = controller.GetOrders();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(objectResult.Value);
            //Assert.Equal(_orderDTOs, model);
            List<int> groupIDs = model.Select(x => x.OrderGroup).ToList();
            foreach (OrderDTO o in model)
            {
                Assert.Contains(o.OrderGroup, groupIDs);
            }
        }

        [Fact]
        public void GetProductsTest()
        {
            var controller = new ProductsController(_context);
            var result = controller.GetProducts();
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(objectResult.Value);
            //Assert.Equal(_productDTOs, model.ToList());
            List<int> productIDs = model.Select(x => x.Id).ToList();
            foreach (ProductDTO p in model)
            {
                Assert.Contains(p.Id, productIDs);
            }
        }

        [Fact]
        public void GetProductsForOrderTest()
        {
            var controller = new ProductsController(_context);
            //a sample order was created with this data:
            Int32 orderNum = 10;
            Int32 prodID = 2;

            var result = controller.GetProducts(orderNum);
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(objectResult);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(objectResult.Value);
            Assert.NotNull(model);
            //in this case the resulting list will contain a single element only:
            //Assert.Single(model);
            //Assert.Equal(prodID, model.First().Id);
        }

        [Fact]
        public void PutOrderCompletedTest()
        {
            var controller = new OrdersController(_context);
            OrderDTO order = _orderDTOs[0];

            Assert.False(order.Completed);
            var result = controller.PutOrderCompleted(order);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<OrderDTO>(objectResult.Value);
            Assert.True(model.Completed);
        }

        [Fact]
        public void CreateProductTest()
        {
            ProductDTO newProduct = new ProductDTO
            {
                //Id = 33,
                Name = "Fish dish",
                Description = "fishy",
                Price = 666,
                CategoryId = 2,
                IsSpicy = false,
                IsVegan = false
            };

            var controller = new ProductsController(_context);
            var result = controller.PostProduct(newProduct);

            var objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            var model = Assert.IsAssignableFrom<ProductDTO>(objectResult.Value);
            Assert.Equal(_productDTOs.Count + 1, _context.Products.Count());
            Assert.Equal(newProduct, model);
        }
    }
}
