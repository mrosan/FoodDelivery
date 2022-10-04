using FoodDeliveryAPI.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<Manager> _signInManager;

        public AccountController(SignInManager<Manager> signInManager)
        {
            _signInManager = signInManager;
        }

        ~AccountController()
        {
            Dispose(false);
        }

        [HttpGet("login/{userName}/{userPassword}")]
        public async Task<IActionResult> Login(String userName, String userPassword)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userName, userPassword, false, false);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Nem sikerült belépni! - rossz jelszó");
                    return Forbid();
                }
                return Ok();
            }
            catch
            {
                Console.WriteLine("Nem sikerült belépni! - szerver hiba");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}