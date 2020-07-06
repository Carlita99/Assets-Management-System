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
    public class BranchController : Controller
    {
        private readonly UserManager<Company> _userManager;
        private readonly SignInManager<Company> _signInManager;
        private readonly Context _context;
        public BranchController(UserManager<Company> userManager, SignInManager<Company> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Add(string address,string phone)
        {
            if(String.IsNullOrEmpty(address)|| String.IsNullOrEmpty(phone))
            {
                ViewData["Error"] = "All fields are required";
                return View();
            }
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int id = int.Parse(await _userManager.GetUserIdAsync(company));
            Branch branch = new Branch();
            branch.Company = company;
            branch.Address = address;
            branch.PhoneNumber = phone;
            await _context.AddAsync(branch);
            var changed = await _context.SaveChangesAsync();
            if (changed > 0)
            {
                return RedirectToAction("CompanyProfile", "Auth");
            }
            ViewData["Error"] = "Oops, an error occured. Please Try Again.";
            return View();
        }
        public async Task<IActionResult> Edit(int id,string address, string phone) {
            Branch branch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == id);
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));

            if (branch == null || companyId != branch.Company.Id) {
                ViewData["Error"] = "You don't have access to edit this item.";
                return View();
            }

            if (String.IsNullOrEmpty(address) || String.IsNullOrEmpty(phone))
            {
                ViewData["Error"] = "All fields are required";
                ViewData["branch"] = branch;
                return View();
            }

            branch.Address = address;
            branch.PhoneNumber = phone;
            var changed = await _context.SaveChangesAsync();
            if (changed > 0)
            {
                return RedirectToAction("CompanyProfile", "Auth");
            }
            ViewData["Error"] = "Oops, an error occured. Please Try Again.";
            return View();
        }
    }
}
