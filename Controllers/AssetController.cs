using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class AssetController : Controller
    {

        private readonly UserManager<Company> _userManager;
        private readonly SignInManager<Company> _signInManager;
        private readonly Context _context;
        private readonly IAssetService _assetService;
        public AssetController(IAssetService assetService, UserManager<Company> userManager, SignInManager<Company> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _assetService = assetService;
        }
        public async Task<IActionResult> Index(string name)
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int id = int.Parse(await _userManager.GetUserIdAsync(company));
            if (string.IsNullOrEmpty(name))
                company = await _assetService.GetPopulatedCompany(id);
            else
                company = await _assetService.GetCompanyAndStaffModel(id,name);

            ViewData["company"] = company;
            return View();
        }

        public async Task<IActionResult> Asset(int id)
        {

            Asset asset = await _assetService.GetPopulatedAsset(id);
            if (asset == null)
            {
                ViewData["Error"] = "Asset not found";

                return View();
            }

            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));
            if (companyId != asset.Company.Id)
                ViewData["Error"] = "You don't have access to this asset.";
            else
                ViewData["asset"] = asset;

            return View();
        }


        public async Task<IActionResult> AddAsset(bool rented, string model, DateTime aquired, string description, float price, int type, int staff, int branch)
        {

            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));

            if (model == null || aquired == null || description == null)
            {
                ViewData["Error"] = "All Properties are required";
                ViewData["staffList"] = await _context.Staff.Where(s => s.Branch.Company.Id == companyId).ToListAsync();
                ViewData["companyBranches"] = await _context.Branches.Where(b => b.Company.Id == companyId).ToListAsync();
                ViewData["assetTypes"] = await _context.AssetTypes.ToListAsync();
                return View();
            }

            var changed = await _assetService.AddAsset(rented, model, aquired, description, price, type, staff, branch, company);
            if (changed > 0)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = "Oops, an error occured. Please Try Again.";
            return View();

        }

        public async Task<IActionResult> EditAsset(int Id, bool rented, string model, DateTime aquired, string description, float price, int type, int staff, int branch)
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));
            Asset asset = await _assetService.GetPopulatedAsset(Id);
            if (asset == null)
            {
                ViewData["Error"] = "Asset doesn't exist.";
                return View();
            }
            if (asset.Company.Id != companyId)
            {
                ViewData["Error"] = "You don't have access to edit this item.";
                return View();
            }

            if (model == null || aquired == null || description == null)
            {
                ViewData["staffList"] = await _context.Staff.Where(s => s.Branch.Company.Id == companyId).ToListAsync();
                ViewData["companyBranches"] = await _context.Branches.Where(b => b.Company.Id == companyId).ToListAsync();
                ViewData["assetTypes"] = await _context.AssetTypes.ToListAsync();
                ViewData["asset"] = asset;
                return View();
            }

            var changed = await _assetService.EditAsset(asset, rented, model, aquired, description, price, type, staff, branch, company);
            if (changed > 0)
            {
                return RedirectToAction("Index");
            }
            ViewData["Error"] = "Oops, an error occured. Please Try Again.";
            return View();

        }

        public async Task<IActionResult> DisposeAsset(int Id)
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int companyId = int.Parse(await _userManager.GetUserIdAsync(company));
            Asset asset = await _assetService.GetPopulatedAsset(Id);
            if (asset == null)
            {
                ViewData["Error"] = "Asset doesn't exist.";
                return View();
            }
            if (asset.Company.Id != companyId)
            {
                ViewData["Error"] = "You don't have access to edit this item.";
                return View();
            }
            var changed = await _assetService.DisposeAsset(asset);
            if (changed > 0)
            {
                ViewData["Error"] = "Oops, an error occured. Please Try Again.";
            }
            return RedirectToAction("Index");
        }
    }
}
