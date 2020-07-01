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
    public class Context: IdentityDbContext <Company,IdentityRole<int>,int>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        DbSet<Asset> Assets { get; set; }
        DbSet<AssetType> AssetTypes { get; set; }
        DbSet<Branch> Branches { get; set; }
        //DbSet<Company> Companies { get; set; }
        DbSet<CompanyType> CompanyTypes { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<ContractType> ContractTypes { get; set; }
        DbSet<Staff> Staff { get; set; }
      

    }
}
