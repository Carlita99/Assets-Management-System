using AssetManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Services
{
    public interface IStaffService
    {
        public Task<int> AddStaff(string firstName, string lastName, string gender, string email, string address, Branch branch);
        public Task<Staff> GetStaffById(int id,Company company);
        public Task<int> EditStaff(string firstName, string lastName, string gender, string email, string address,Branch branch, Staff staff);
    }
}
