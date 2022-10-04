using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodDeliveryService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Price { get; set; }
        public bool Completed { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string TelephoneNr { get; set; }

        public DateTime DateSubmitted { get; set; }
        public DateTime DateCompleted { get; set; }
        public int OrderGroup { get; set; }
    }
}
