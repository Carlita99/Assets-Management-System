using AssetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Services
{
   public interface IAssetService
    {
        public  Task<Company> GetPopulatedCompany(int id);
        public  Task<Company> GetCompanyAndStaffModel(int id, string model);

        public Task<Asset> GetPopulatedAsset(int id);
        public Task<int> AddAsset(bool rented, string model, DateTime aquired, string description, float price, int type, int staff, int branch, Company company);
        public Task<int> EditAsset(Asset asset, bool rented, string model, DateTime aquired, string description, float price, int type, int staff, int branch, Company company);
        public Task<int> DisposeAsset(Asset asset);

    }
}
