using AssetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Services
{
    public interface IBranchService
    {
        public Task<int> Add(string address, string phone,Company company);
        public Task<int> Edit(Branch branch, string address, string phone);
        public Task<Branch> GetBranchById(int id);
    }
}
