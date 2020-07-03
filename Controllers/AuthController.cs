using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Data;
using AssetManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<Company> _userManager;
        private readonly SignInManager<Company> _signInManager;
        private readonly Context _context;
        public AuthController(UserManager<Company> userManager, SignInManager<Company> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Register(string email, string password, string confirmPassword, string name,int CompanyType)
        {
            try
            {
                if (email == null || password == null)
                    throw new Exception("");
                if (password != confirmPassword)
                    throw new Exception("Passwords don't match");
                CompanyType companyType = await _context.CompanyTypes.FirstOrDefaultAsync(t => t.Id == CompanyType);
                Company company = await _userManager.FindByEmailAsync(email);
                if (company != null)
                    throw new Exception("This email is taken by another company");

                company = new Company();
                company.Name = name;
                company.UserName = email;
                company.Email = email;
                company.Type = companyType;
                IdentityResult result = await _userManager.CreateAsync(company, password);
                if (result.Succeeded)
                    return RedirectToAction("Login", "Auth", new { email = email, password = password });
                else
                    throw new Exception("Oops! An error occured. Please try again");
            }
            catch (Exception e)
            {
                ICollection<CompanyType> companyTypes = await _context.CompanyTypes.ToListAsync();
                ViewData["companyTypes"] = companyTypes;

                ViewData["Error"] = e.Message;
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
                 return RedirectToAction("Index", "Asset");
            }
            else
            {
                ViewData["Error"] = "Bad Credentials";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
