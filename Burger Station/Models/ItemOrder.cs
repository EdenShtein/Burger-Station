using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class ItemOrder
    {
        public int ItemId { get; set; }

        [Required]
        public Item Item { get; set; }

        public int OrderId { get; set; }

        [Required]
        public Order Orders { get; set; }
    }
}
