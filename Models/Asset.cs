using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public bool Rented { get; set; }
        public string Model { get; set; }
        public DateTime AquiredDate { get; set; }
        public DateTime DisposedDate { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public AssetType Type { get; set; }
        public Staff Staff { get; set; }
        public Company Company { get; set; }
        public Branch Branch { get; set; }
    }
}
