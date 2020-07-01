﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Models
{
    public class Contract
    {
    public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ContractType Type { get; set; }
        public Asset Asset { get; set; }
        public Company Company { get; set; }

    }
}
