using AssetManagement.Data;
using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Services
{
    public class BranchService : IBranchService
    {
        private readonly Context _context;
        public BranchService(Context context)
        {
            _context = context;
        }
        public async Task<int> Add(string address, string phone, Company company)
        {
            Branch branch = new Branch();
            branch.Company = company;
            branch.Address = address;
            branch.PhoneNumber = phone;
            await _context.AddAsync(branch);
            var changed = await _context.SaveChangesAsync();
            return changed;
        }

       public async Task<int>Edit(Branch branch, string address, string phone)
        {
            branch.Address = address;
            branch.PhoneNumber = phone;
            var changed = await _context.SaveChangesAsync();
            return changed;
        }

        public async Task<Branch> GetBranchById(int id)
        {
           return await _context.Branches.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
