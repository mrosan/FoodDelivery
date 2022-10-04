using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryService.Models;

namespace FoodDeliveryService.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DeliveryContext _context;

        public ProductsController(DeliveryContext context)
        {
            _context = context;
        }
        
        //This is what runs at first on the homepage: it shows the 10 most popular products
        public IActionResult Index()
        {
            ViewBag.Cats = _context.Categories.ToArray();

            var products = from p in _context.Products select p;
            products = products.OrderByDescending(x => x.AmountSold).Take(10);

            return View("Index", products.ToList());
        }

        // Listing products containing searchString in the selectedCategory.
        // Also passes the selected category's name (aesthetic purpose),
        // and the number of pages that the resulting list should be divided on.
        public async Task<IActionResult> List(string searchString, int? selectedCat, string selectedCatName, int? pageNr)
        {
            var products = from p in _context.Products select p;

            //filtering the search results
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }
            if (selectedCat != null)
            {
                products = products.Where(x => x.CategoryId == selectedCat);
                ViewBag.SelectedCategory = selectedCat;
                ViewBag.SelectedCategoryName = selectedCatName;
            }
            ViewBag.Cats = _context.Categories.ToArray();        

            //paging info
                int itemsPerPage = 5;
                if ((products.Count() % itemsPerPage)==0)  ViewBag.PageNumber = products.Count() / itemsPerPage;
                else ViewBag.PageNumber = (products.Count() / itemsPerPage) + 1;
                int prefix = 0;
                if (pageNr != null)
                {
                    prefix = (int)pageNr * itemsPerPage;
                }

            //constructing the ViewModel
            var prodCatVM = new ProductCategoryViewModel();
            prodCatVM.categories = await _context.Categories.ToListAsync();
            prodCatVM.products = await products.ToListAsync();

            //paging info
                prodCatVM.products.RemoveRange(0, prefix);
                int postfix = prodCatVM.products.Count() - itemsPerPage;
                if (postfix > 0)
                {
                    prodCatVM.products.RemoveRange(itemsPerPage, postfix);
                }

            return View(prodCatVM);
        }
    }
}
