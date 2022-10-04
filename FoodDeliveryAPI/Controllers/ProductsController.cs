using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryData;
using FoodDeliveryAPI.Models;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private FoodDeliveryContext _context;

        public ProductsController(FoodDeliveryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        //Get all products
        [HttpGet(Name = "GetProducts")]
        public IActionResult GetProducts()
        {
            try
            {
                return Ok(_context.Products.ToList().Select(p => new ProductDTO
                {
                    Id = p.ProductID,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    IsSpicy = p.IsSpicy,
                    IsVegan = p.IsVegan
                }));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Get all products belonging to a specific order
        [HttpGet("{OrderId}")]
        public IActionResult GetProducts(Int32 OrderId)
        {
            try
            {
                List<Order> orders = _context.Orders.Where(x => x.OrderGroup == OrderId).ToList();
                List<Product> products = new List<Product>();

                foreach ( Order o in orders)
                {
                    Product prdct = _context.Products.Where(x => x.ProductID == o.ProductId).FirstOrDefault();
                    if(prdct != null)
                    {
                        products.Add(prdct);
                    }
                }

                return Ok(products.ToList().Select(p => new ProductDTO
                {
                    Id = p.ProductID,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    IsSpicy = p.IsSpicy,
                    IsVegan = p.IsVegan
                }));
                
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Create a new product
        [HttpPost]
        public IActionResult PostProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                var addedProduct = _context.Products.Add(new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    CategoryId = productDTO.CategoryId,
                    IsSpicy = productDTO.IsSpicy,
                    IsVegan = productDTO.IsVegan,
                    AmountSold = 0
                });

                _context.SaveChanges();

                productDTO.Id = addedProduct.Entity.ProductID;

                return CreatedAtRoute("GetProducts", new { id = addedProduct.Entity.ProductID }, productDTO);
                //return Created(Request.GetUri() + addedProduct.Entity.ProductID.ToString(), productDTO);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        /*
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Int32 id)
        {
            try
            {
                Product product = _context.Products.FirstOrDefault(p => p.ProductID == id);
                if (product == null)
                {
                    return NotFound();
                }  
                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        */
    }
}