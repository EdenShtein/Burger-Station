using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        public ItemType Type { get; set; }

        [RegularExpression(@"^[a-zA-Z][a-zA-Z ]+")]
        public String Name { get; set; }

        [Range(0, 50)]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        public ICollection<Comment> Comments { get; set; }

        [DisplayName("Satisfied Users")]
        public ICollection<User> SatisfiedUsers { get; set; }

        [DisplayName("List of Branches that have the Item")]
        public ICollection<BranchItem> BranchItems { get; set; }
    }
}