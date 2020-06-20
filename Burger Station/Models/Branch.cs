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
        Center
    }

    public class Branch
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The address is required")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z0-9\s]*$")]
        public String Address { get; set; }

        [Required(ErrorMessage = "The city is required")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String City { get; set; }

        [Required(ErrorMessage = "The district is required")]
        public District District { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        public String PhoneNumber { get; set; }

        public ICollection<User> UsersFavorite { get; set; }
    }
}
