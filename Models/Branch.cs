using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Company Company { get; set; }
        public ICollection<Asset> Assets { get; set; }
        public ICollection<Staff> Staff { get; set; }

    }
}
