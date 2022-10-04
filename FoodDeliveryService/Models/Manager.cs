using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FoodDeliveryService.Models
{
    public class Manager : IdentityUser<int>
    {
        /* IdentityUser includes:
         * T Id
		 * string UserName
		 * string PasswordHash (aka UserPassword)
		 * string Email
		 * string PhoneNumber
		 * string SecurityStamp (aka UserChallange) 
         * */

        //The full name of the user
        public string Name { get; set; }
    }
}
