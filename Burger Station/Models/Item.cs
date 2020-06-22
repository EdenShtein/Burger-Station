using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public enum ItemType
    {
        Food,
        Drink
    }

    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name is required")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z ]+")]
        public String Name { get; set; }

        [Required(ErrorMessage = "The price is required")]
        [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "The type of the item is required")]
        public ItemType Type { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<ItemOrder> OrdersOfItem { get; set; }
    }
}
