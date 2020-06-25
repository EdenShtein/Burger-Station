using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Burger_Station.Models;

namespace Burger_Station.Models
{
    public class BranchItem
    {
        public int BranchId { get; set; }

        public Branch Branch { get; set; }

        public int ItemId { get; set; }

        public Item Item { get; set; }
    }
}
