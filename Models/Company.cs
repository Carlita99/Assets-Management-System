using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Models
{
    public class Company:IdentityUser<int>
    {
        public string Name { get; set; }
        public ICollection <Branch> Branches { get; set; }
        public ICollection<Asset> Assets { get; set; }
        public CompanyType Type { get; set; }
    }
}
