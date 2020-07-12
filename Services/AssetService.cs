using AssetManagement.Data;
using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace AssetManagement.Services
{
    public class AssetService:IAssetService
    {
     
        private readonly Context _context;
        public AssetService( Context context)
        {
            _context = context;
        }
        public async Task<Company> GetPopulatedCompany(int id)
        {
            return await _context.Users
                .Include(c => c.Assets).ThenInclude(a => a.Type)
                .Include(c => c.Assets).ThenInclude(a => a.Staff)
                .Include(c => c.Assets).ThenInclude(a => a.Branch)
                .SingleOrDefaultAsync(c => c.Id == id);

        }

        public async Task<Asset> GetPopulatedAsset(int id)
        {
            return await _context.Assets
                        .Include(a => a.Type)
                        .Include(a => a.Branch).ThenInclude(b => b.Company)
                        .Include(a => a.Staff)
                        .Include(a => a.Company)
                        .FirstOrDefaultAsync(asset => asset.Id == id);
        }

        public async Task<int> AddAsset(bool rented, string model, DateTime aquired, string description, float price, int type, int staff, int branch,Company company)
        {
            AssetType assetType = await _context.AssetTypes.FirstOrDefaultAsync(t => t.Id == type);
            Branch companyBranch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == branch);
            Staff responsible = await _context.Staff.FirstOrDefaultAsync(s => s.Branch.Company.Id == company.Id);

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
                Company = company
            };
            await _context.Assets.AddAsync(asset);
            var changed = await _context.SaveChangesAsync();
            return changed;
        }
        public async Task<int> EditAsset(Asset asset, bool rented, string model, DateTime aquired, string description, float price, int type, int staff, int branch, Company company) {

            AssetType assetType = await _context.AssetTypes.FirstOrDefaultAsync(t => t.Id == type);
            Branch companyBranch = await _context.Branches.FirstOrDefaultAsync(b => b.Id == branch);
            Staff responsible = await _context.Staff.FirstOrDefaultAsync(s => s.Branch.Company.Id == company.Id);

            asset.Rented = rented;
            asset.Model = model;
            asset.AquiredDate = aquired;
            asset.Description = description;
            asset.Price = price;
            asset.Type = assetType;
            asset.Branch = companyBranch;
            asset.Staff = responsible;

            var changed = await _context.SaveChangesAsync();
            return changed;

        }
        public async Task<int> DisposeAsset(Asset asset) {
            asset.DisposedDate = DateTime.Now;
            var changed = await _context.SaveChangesAsync();
            return changed;
        }

        public async Task<Company> GetCompanyAndStaffModel(int id, string model)
        {
            return await _context.Users
              .IncludeFilter(c => c.Assets.Where(a => a.Model == model).Select(a=>a.Staff))
              .IncludeFilter(c => c.Assets.Where(a => a.Model == model).Select(a => a.Branch))
              .IncludeFilter(c => c.Assets.Where(a => a.Model == model).Select(a => a.Type))
              .SingleOrDefaultAsync(c => c.Id == id);

        }
    }
}
