using AssetManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Contract = AssetManagement.Models.Contract;

namespace AssetManagement.Data
{
    public class Context : IdentityDbContext<Company, IdentityRole<int>, int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetType> AssetTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        //DbSet<Company> Companies { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<Staff> Staff { get; set; }


    }
}
