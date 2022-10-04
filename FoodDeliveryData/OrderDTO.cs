using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryData
{
    public class OrderDTO
    {
        public int OrderGroup { get; set; }
        public bool Completed { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateCompleted { get; set; }
        public int Sum { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string TelephoneNr { get; set; } 
    }
}
