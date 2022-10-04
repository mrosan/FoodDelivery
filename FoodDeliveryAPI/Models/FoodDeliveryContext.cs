using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryAPI.Models
{
    public class FoodDeliveryContext : IdentityDbContext<Manager, IdentityRole<int>, int>
    {
        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Manager>().ToTable("Managers");
            // instead of the table name "AspNetUsers"
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        //public DbSet<Manager> Managers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
