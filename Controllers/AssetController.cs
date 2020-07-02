using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AssetManagement.Data;
using AssetManagement.Models;
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
        public AssetController(UserManager<Company> userManager, SignInManager<Company> signInManager, Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Company company = await _userManager.GetUserAsync(HttpContext.User);
            int id = int.Parse(await _userManager.GetUserIdAsync(company));
            company = await _context.Users
                .Include(c => c.Assets).ThenInclude(a => a.Type)
                .Include(c => c.Assets).ThenInclude(a => a.Staff)
                .Include(c => c.Assets).ThenInclude(a => a.Branch)
                .SingleOrDefaultAsync(c => c.Id == id);
            ViewData["company"] = company;
            return View();
        }

        public async Task<IActionResult> Asset(int id)
        {

            Asset asset = await _context.Assets
                        .Include(a => a.Type)
                        .Include(a => a.Branch).ThenInclude(b => b.Company)
                        .Include(a => a.Staff)
                        .Include(a => a.Company)
                        .FirstOrDefaultAsync(asset => asset.Id == id);
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

            AssetType assetType = await _context.AssetTypes.FirstOrDefaultAsync(t => t.Id == type);
            Branch companyBranch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == branch);
            Staff responsible = await _context.Staff.FirstOrDefaultAsync(s => s.Branch.Company.Id == companyId);

            Asset asset = new Asset()
            {
                Rented = rented,
                Model = model,
                AquiredDate = aquired,
                Description = description,
                Price = price,
                Branch = companyBranch,
                Staff = responsible,
                Type = assetType,
                Company=company
            };
            await _context.Assets.AddAsync(asset);
            var changed = await _context.SaveChangesAsync();
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
            Asset asset = await _context.Assets
                .Include(a => a.Company)
                .Include(a => a.Staff)
                .Include(a => a.Branch)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.Id == Id);
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

            AssetType assetType = await _context.AssetTypes.FirstOrDefaultAsync(t => t.Id == type);
            Branch companyBranch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == branch);
            Staff responsible = await _context.Staff.FirstOrDefaultAsync(s => s.Branch.Company.Id == companyId);

            asset.Rented = rented;
            asset.Model = model;
            asset.AquiredDate = aquired;
            asset.Description = description;
            asset.Price = price;
            asset.Type = assetType;
            asset.Branch = companyBranch;
            asset.Staff = responsible;

            var changed = await _context.SaveChangesAsync();
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
            Asset asset = await _context.Assets
                .Include(a => a.Company)
                .Include(a => a.Staff)
                .Include(a => a.Branch)
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.Id == Id);
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
            asset.DisposedDate = DateTime.Now;
            var changed = await _context.SaveChangesAsync();
            if (changed > 0)
            {
                ViewData["Error"] = "Oops, an error occured. Please Try Again.";
            }
            return RedirectToAction("Index");
        }
    }
}
