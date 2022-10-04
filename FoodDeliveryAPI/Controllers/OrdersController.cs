using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryData;
using FoodDeliveryAPI.Models;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private FoodDeliveryContext _context;

        public OrdersController(FoodDeliveryContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        // Getting all of the orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            try
            {
                List<OrderDTO> orderlist = _context.Orders
                    .GroupBy(x => x.OrderGroup)
                    .ToList()
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

                return Ok(orderlist);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
       

        [HttpPut]
        public IActionResult PutOrderCompleted([FromBody] OrderDTO orderDTO)
        {
            try
            {
                //if only a single Order is needed:
                //Order order = _context.Orders.FirstOrDefault(o => o.OrderGroup == orderDTO.OrderGroup);

                List<Order> orders = _context.Orders.Where(o => o.OrderGroup == orderDTO.OrderGroup).ToList();

                if (orders == null)
                {
                    return NotFound();
                }

                foreach (Order o in orders)
                {
                    if (!o.Completed)
                    {
                        o.Completed = true;
                        o.DateCompleted = orderDTO.DateCompleted;
                        Product p = _context.Products.FirstOrDefault(x => x.ProductID == o.ProductId);
                        if (p != null)
                        {
                            p.AmountSold++;
                        }
                    }
                    
                }    
                _context.SaveChanges();
                orderDTO.Completed = true;
                return Ok(orderDTO);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
    }
}