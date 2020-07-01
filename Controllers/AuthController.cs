using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<Company> _userManager;
        private SignInManager<Company> _signInManager;

        public AuthController(UserManager<Company> userManager, SignInManager<Company> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Register(string email, string password, string name)
        {
            try
            {
                if (email == null || password == null)
                    throw new Exception();

                Company company = await _userManager.FindByEmailAsync(email);
                if (company != null)
                    throw new Exception();
                company = new Company();
                company.Name = name;
                company.UserName = email;
                company.Email = email;
                IdentityResult result = await _userManager.CreateAsync(company, password);
                if (result.Succeeded)
                    return RedirectToAction("Login", "Auth", new { email = email, password = password });
                else
                    throw new Exception();
            }
            catch
            {
                Console.WriteLine("Error login");
                ViewBag.Error = "This email is taken by another company";
                return View();
            }

        }

        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == null || password == null)
                return View();
            var result = await _signInManager.PasswordSignInAsync(email, password, true, false);


            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Bad Credentials";
                return View();
            }
        }
    }
}
