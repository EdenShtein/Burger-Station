using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("First Name")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Last Name")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String LastName { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 11)] // X@gmail.com
        [RegularExpression(@"^[A-Za-z0-9\s]*$")]
        public String Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 5)] // 12345 - 1234567890
        public String Password { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        public String PhoneNumber { get; set; }

        [DisplayName("Favorite Branch")]
        public Branch FavoriteBranch { get; set; }

        [DisplayName("Orders History")]
        public ICollection<Order> Orders { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
