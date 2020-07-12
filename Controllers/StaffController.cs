using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Data;
using AssetManagement.Models;
using AssetManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Controllers
{
    [Authorize]
    public class StaffController : Controller
    {

        private readonly UserManager<Company> _userManager;
        private readonly SignInManager<Company> _signInManager;
        private readonly Context _context;
        private readonly IBranchService _branchService;
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService, IBranchService branchService, UserManager<Company> userManager, SignInManager<Company> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _branchService = branchService;
            _staffService = staffService;

        }
        public async Task<IActionResult> Index()
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));

            ICollection<Staff> staff = await _context.Staff.Where(s => s.Branch.Company.Id == companyId)
                                      .Include(s => s.Branch)
                                      .ToListAsync();

            ViewData["staff"] = staff;
            return View();
        }

        public async Task<IActionResult> AddStaff(string firstName, string lastName, string gender, string email, string address, int branchId)
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));

            if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName) || String.IsNullOrEmpty(gender) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(address))
            {
                ViewData["error"] = "All fields are required";
                ICollection<Branch> branches = await _context.Branches.Where(b => b.Company.Id == companyId).ToListAsync();
                ViewData["branches"] = branches;
                return View();
            }
            Branch branch = await _branchService.GetBranchById(branchId);
            var result = await _staffService.AddStaff(firstName, lastName, gender, email, address, branch);
            if (result < 0)
            {
                ViewData["error"] = "Oops! An error occured. Please try again.";
                return View();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditStaff(int id, string firstName, string lastName, string gender, string email, string address, int branchId)
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));
            if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName) || String.IsNullOrEmpty(gender) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(address))
            {
                Staff s = await _context.Staff.FirstOrDefaultAsync((s) => s.Id == id);
                ViewData["staff"] = s;

                ViewData["error"] = "All fields are required";
                ICollection<Branch> branches = await _context.Branches.Where(b => b.Company.Id == companyId).ToListAsync();
                ViewData["branches"] = branches;
                return View();
            }

            Staff staff = await _staffService.GetStaffById(id, company);
            if (staff == null)
            {
                ViewData["Error"] = "You don't have access to this content";
                return View();
            }
            Branch branch = await _branchService.GetBranchById(branchId);
            var result = await _staffService.EditStaff(firstName, lastName, gender, email, address, branch, staff);
            if (result < 0)
            {
                ViewData["error"] = "Oops! An error occured. Please try again.";
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
