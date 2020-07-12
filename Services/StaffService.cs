using AssetManagement.Data;
using AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Services
{
    public class StaffService : IStaffService
    {
        private readonly Context _context;
        public StaffService(Context context)
        {
            _context = context;
        }
        public async Task<int> AddStaff(string firstName, string lastName, string gender, string email, string address, Branch branch)
        {
            Staff staff = new Staff();
            staff.Branch = branch;
            staff.FirstName = firstName;
            staff.LastName = lastName;
            staff.Email = email;
            staff.Gender = gender;
            staff.Address = address;

            await _context.AddAsync(staff);
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Staff> GetStaffById(int id,Company company)
        {
           return await _context.Staff.FirstOrDefaultAsync((s) => s.Id == id && s.Branch.Company.Id == company.Id);
        }

        public async Task<int> EditStaff(string firstName, string lastName, string gender, string email, string address, Branch branch,Staff staff)
        {
            staff.Branch = branch;
            staff.FirstName = firstName;
            staff.LastName = lastName;
            staff.Email = email;
            staff.Gender = gender;
            staff.Address = address;


            var result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
