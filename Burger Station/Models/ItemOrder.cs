using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class ItemOrder
    {
        [DisplayName("Item")]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        [DisplayName("Order")]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        [DisplayName("Branch")]
        public int BranchId { get; set; }

        public Branch Branch { get; set; }
    }
}
