using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public String Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public String Address { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public String City { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public String PhoneNumber { get; set; }

        public ICollection<User> UsersFavorite { get; set; }
    }
}
