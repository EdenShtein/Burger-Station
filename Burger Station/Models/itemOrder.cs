using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class itemOrder
    {
        public int IdItem { get; set; }
        public Item Item { get; set; }
        public int IdOrder { get; set; }
        public Order Orders { get; set; }
    }
}
