using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodDeliveryService.Models
{
    public class OrderViewModel
    {
        [BindNever] //prevents the user from supplying values for this property in an HTTP request
        public int OrderId { get; set; }

        [BindNever]
        public IEnumerable<CartItem> itemCollection { get; set; }

        [BindNever]
        public bool Completed { get; set; }

        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(60, ErrorMessage = "Name's length can be 60 characters at maximum.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an address!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter a telephone number!")]
        [Phone(ErrorMessage = "The number's format is invalid.")]
        public string TelephoneNr { get; set; }

        //[Required(ErrorMessage = "Please enter an e-mail address!")]
        //[EmailAddress(ErrorMessage = "The e-mail's format is invalid.")]
        //[DataType(DataType.EmailAddress)]
        //public String Email { get; set; }
    }
}
