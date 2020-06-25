using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Burger_Station.Models
{
    public enum UserType
    {
        Member,
        Admin
    }

    public class User
    {
        public int Id { get; set; }

        public UserType Type { get; set; }

        [DisplayName("First Name")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String FirstName { get; set; }

        [DisplayName("Last Name")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [StringLength(10, MinimumLength = 5)] // 00000 - 9999999999
        public String Password { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [DisplayName("Favorite Item on the Menu")]
        public Item FavoriteItem { get; set; }
    }
}