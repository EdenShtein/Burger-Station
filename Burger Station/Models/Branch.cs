using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public enum District
    {
        North,
        South,
        Center,
    }

    public class Branch
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The address is required")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z0-9 ][a-zA-Z0-9 ]+")]
        public String Address { get; set; }

        [Required(ErrorMessage = "The city is required")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z ]+")]
        public String City { get; set; }

        [Required(ErrorMessage = "The District of the branch is required")]
        public District District { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{8})$")]
        public String PhoneNumber { get; set; }

        public ICollection<User> UsersFavorite { get; set; }
    }
}
