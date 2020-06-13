using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class ItemOrder
    {
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public int OrderId { get; set; }

        public Order Orders { get; set; }
    }
}
