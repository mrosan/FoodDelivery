using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryService.Models;

namespace FoodDeliveryService.Controllers
{
    public class CartController : Controller
    {
        private readonly DeliveryContext _context;

        //This amount specifies the maximum cost of food that the user can order.
        private static int maximumCost = 20000;

        public CartController(DeliveryContext context)
        {
            _context = context;
        }

        //Shows the cart.
        public ViewResult Index(string returnUrl)
        {
            ViewBag.Cats = _context.Categories.ToArray();
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        //Add Product by ID to the Cart. Other parameter is the url from which the user visited the cart.
        //(model binding, associate incoming form POST variables with the parameters)
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _context.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            ViewBag.Cats = _context.Categories.ToArray();

            if (product != null)
            {
                Cart cart = GetCart();
                if (cart.CartValue() + product.Price <= maximumCost)
                {
                    cart.AddItem(product, 1);
                    SaveCart(cart);
                }
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        //Remove Product by ID from the Cart. Other parameter is the url from which the user visited the cart.
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = _context.Products.Where(p => p.ProductID == productId).FirstOrDefault();
            ViewBag.Cats = _context.Categories.ToArray();

            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveItems(product);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        //Empties the cart.
        //Return action depends on whether an order was finished or canceled.
        public IActionResult ResetCart(bool? orderFinished)
        {
            Cart cart = GetCart();
            cart.ClearCart();
            SaveCart(cart);

            if (orderFinished == null || ! (bool)orderFinished)
            {
                return RedirectToAction("Index", "Products");
            } else
            {
                ViewBag.Message = "Your order has been successfully sent!";
                ViewBag.Cats = _context.Categories.ToArray();
                //if the orderFinished==true, it means that the cart became empty after that
                //thus we should return with the "Result" view
                return View("Result");
            }
        }

        //Gets the cart from the Session
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        //Sets the cart to the Session
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
    }
}