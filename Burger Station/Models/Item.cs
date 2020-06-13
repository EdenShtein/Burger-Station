using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Item
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public double Price { get; set; }
        
        public ICollection<Comment> Comments { get; set; }
        
        public ICollection<ItemOrder> OrdersOfItem { get; set; }
    }
}
