using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryService.Models;

namespace FoodDeliveryService.Controllers
{
    public class OrderController : Controller
    {
        private readonly DeliveryContext _context;

        public OrderController(DeliveryContext context)
        {
            _context = context;
            ViewBag.Cats = _context.Categories.ToArray();
        }


        //GET action for submitting an order.
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Cats = _context.Categories.ToArray();

            OrderViewModel orderVM = new OrderViewModel();
            return View("Index", orderVM);
        }

        //POST action for submitting an order.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(OrderViewModel order)
        {
            ViewBag.Cats = _context.Categories.ToArray();
            if (order == null)
            {
                return RedirectToAction("Index", "Products");
            }

            //Take the cart from the session
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();

            //take the list of ordered products
            order.itemCollection =  cart.getItems;

            //if there are no ordered products, return to home page
            if(order.itemCollection.Count() == 0)
            {
                return RedirectToAction("Index", "Products");
            }

            //server-side checks
            //(checking remaining/available products would be here for example)
            if (!ModelState.IsValid)
                return View("Index", order);

            //create special Order-Group number
            bool unique = false;
            int rInt = 0;
            while (!unique)
            {
                Random r = new Random();
                rInt = r.Next(0, 10000);
                int num = _context.Orders.Where(x => x.OrderGroup == rInt).Count();
                unique = ( num > 0) ? false : true;
            }
            

            //update database
            foreach (CartItem item in order.itemCollection)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    Order o = new Order
                    {
                        ProductId = item.Item.ProductID,
                        Price = item.Item.Price,
                        Completed = false,
                        Name = order.Name,
                        Address = order.Address,
                        TelephoneNr = order.TelephoneNr,
                        DateSubmitted = DateTime.Now,
                        OrderGroup = rInt
                    };
                    _context.Orders.Add(o);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("ResetCart", "Cart", new { orderFinished = true });
        }
    }
}