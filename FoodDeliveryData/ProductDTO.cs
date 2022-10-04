using System;
using System.Collections.Generic;
using System.Text;

namespace FoodDeliveryData
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsVegan { get; set; }
        public bool IsSpicy { get; set; }
        //public int AmountSold { get; set; }
    }
}
