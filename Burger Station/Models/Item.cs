using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<ItemOrder> OrdersOfItem { get; set; }
    }
}
