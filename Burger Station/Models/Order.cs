using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The total price is required")]
        [DisplayName("Total Price")]
        [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public double TotalPrice { get; set; }

        public ICollection<ItemOrder> ItemsOrders { get; set; }
    }
}
