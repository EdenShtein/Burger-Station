using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class itemOrder
    {
        public int idItem { get; set; }
        public Item item { get; set; }
        public int idOrder { get; set; }
        public Order order { get; set; }
    }
}
