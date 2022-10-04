using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryService.Models
{
    public class ProductCategoryViewModel
    {
        public List<Product> products;
        public List<Category> categories;
        //public SelectList catNames;
        //public string ProductCategory { get; set; }      
    }
}
