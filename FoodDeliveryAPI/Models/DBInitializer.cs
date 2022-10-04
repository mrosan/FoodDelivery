using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FoodDeliveryData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Models
{     
    public static class DBInitializer
    {
        private static FoodDeliveryContext _context;
        private static UserManager<Manager> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;

        //public static void Initialize(FoodDeliveryContext context, UserManager<Manager> userManager)
        public static void Initialize(FoodDeliveryContext context, UserManager<Manager> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            //_roleManager = roleManager;

            _context.Database.EnsureCreated();

            if (_context.Products.Any())
            {
                return;
            }


            //initialize users:
            var adminUser = new Manager
            {
                //Id = 666,
                UserName = "admin",
                Name = "Adminisztrátor",
                Email = "admin@example.com",
                PhoneNumber = "+36123456789"
            };
            var adminPassword = "Almafa123";
            //var adminRole = new IdentityRole<int>("administrator");

            var result1 = _userManager.CreateAsync(adminUser, adminPassword).Result;
            //var result2 = _roleManager.CreateAsync(adminRole).Result;
            //var result3 = _userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;


            //initialize categories:
            var categories = new Category[]
            {
            new Category { Name = "Menus" },            //1
            new Category { Name = "Main Dishes" },      //2
            new Category { Name = "Drinks" },           //3
            new Category { Name = "Soups" },            //4
            new Category { Name = "Pizzas" },           //5
            new Category { Name = "Hamburgers" },       //6
            new Category { Name = "Desserts" },         //7
            };

            foreach (Category cat in categories)
            {
                _context.Categories.Add(cat);
            }
            _context.SaveChanges();


            //initialize orders:
            var orders = new Order[]
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

                    Name = "Customer",
                    Address = "Addr",
                    TelephoneNr = "2035677346",

                    DateSubmitted = DateTime.Now,
                    OrderGroup = 23
                },
                new Order
                {
                    ProductId = 1,
                    Price = 900,
                    Completed = false,

                    Name = "Solo order",
                    Address = "Sample address",
                    TelephoneNr = "20/53678543",

                    DateSubmitted = DateTime.Now,
                    OrderGroup = 10
                },
            };

            foreach (Order o in orders)
            {
                _context.Orders.Add(o);
            }
            _context.SaveChanges();


            //initialize products:
            var products = new Product[]
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
                Name = "Baingan bharta",
                Description = "Roasted eggplant mashed together with a variety of other vegetables and spices, served with flatbread.",
                Price = 1800,
                CategoryId = 2,
                IsVegan = true,
                IsSpicy = true,
                AmountSold = 0
            },

            new Product
            {
                Name = "Stir Fried Tofu with Rice",
                Description = "A simple stir-fry with tofu and Oriental sauces. Served with some fried rice to make a wholesome meal.",
                Price = 1500,
                CategoryId = 2,
                IsVegan = true,
                IsSpicy = true,
                AmountSold = 9
            },

            new Product
            {
                Name = "Chicken with Chestnuts",
                Description = "This earthy recipe is perfect for a holiday feast.",
                Price = 900,
                CategoryId = 2,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 3
            },

            new Product
            {
                Name = "Coca Cola",
                Description = "not Pepsi",
                Price = 200,
                CategoryId = 3,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 0
            },

            new Product
            {
                Name = "Pepsi",
                Description = "not Coca Cola",
                Price = 200,
                CategoryId = 3,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 0
            },

            new Product
            {
                Name = "Mineral water",
                Description = "watery",
                Price = 100,
                CategoryId = 3,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 4
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
            },

            new Product
            {
                Name = "Fairy Feast",
                Description = "Salt, Pepper, and a dash of Magic.",
                Price = 17500,
                CategoryId = 1,
                IsVegan = true,
                IsSpicy = true,
                AmountSold = 0
            },

            new Product
            {
                Name = "Chocholate cake",
                Description = "Paradise in soft and creamy form.",
                Price = 790,
                CategoryId = 7,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 0
            },

            new Product
            {
                Name = "Hawaii pizza",
                Description = "Warning!!! Pineapples!",
                Price = 700,
                CategoryId = 5,
                IsVegan = false,
                IsSpicy = false,
                AmountSold = 0
            },

            new Product
            {
                Name = "Cheeseburger",
                Description = "As simple as it gets.",
                Price = 350,
                CategoryId = 6,
                IsVegan = false,
                IsSpicy = true,
                AmountSold = 7
            },

            new Product
            {
                Name = "Hamburger",
                Description = "Even simpler than the cheeseburger.",
                Price = 290,
                CategoryId = 6,
                IsVegan = false,
                IsSpicy = false,
                AmountSold = 0
            },

            new Product
            {
                Name = "Tomato soup",
                Description = "Red and soupy.",
                Price = 490,
                CategoryId = 4,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 2
            },

            new Product
            {
                Name = "Fruity soup",
                Description = "You have to buy this one.",
                Price = 690,
                CategoryId = 4,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 3
            },

            new Product
            {
                Name = "Shark fin soup",
                Description = "No teeth, we promise!",
                Price = 9990,
                CategoryId = 4,
                IsVegan = false,
                IsSpicy = false,
                AmountSold = 2
            },

            new Product
            {
                Name = "Hot and Sour soup",
                Description = "Some kind of Chinese stuff, probably.",
                Price = 990,
                CategoryId = 4,
                IsVegan = true,
                IsSpicy = true,
                AmountSold = 3
            },

            new Product
            {
                Name = "Ulava charu / horsegram lentil soup",
                Description = "This traditional rasam-like soup from Andhra Pradesh is made with horse gram lentils or ulavalu, which are a good source of protein, calcium and iron. Like rasam, it gets its tangy taste from tamarind, and aroma from curry leaves and mustard seeds.",
                Price = 690,
                CategoryId = 4,
                IsVegan = true,
                IsSpicy = false,
                AmountSold = 3
            },

            new Product
            {
                Name = "Currywurst",
                Description = "Yummy.",
                Price = 350,
                CategoryId = 2,
                IsVegan = false,
                IsSpicy = true,
                AmountSold = 7
            },

            new Product
            {
                Name = "Grandpa's Chili Cocktail",
                Description = "Is that drink spicy?!",
                Price = 450,
                CategoryId = 3,
                IsVegan = true,
                IsSpicy = true,
                AmountSold = 0
            },

            };
            foreach (Product p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();


        }
    }
        

}
