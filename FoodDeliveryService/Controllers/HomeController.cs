using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodDeliveryService.Models;

namespace FoodDeliveryService.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeliveryContext _context;

        public HomeController(DeliveryContext context)
        {
            _context = context;
        }

        //GET: all Products
        public IActionResult Index()
        {
            ViewBag.Cats = _context.Categories.ToArray();
            return View("Index", _context.Products.ToList());

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
