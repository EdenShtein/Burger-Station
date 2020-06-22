﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        [Required(ErrorMessage = "The user's first name is required")]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("First Name")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "The user's last name is required")]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Last Name")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public String LastName { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
        public String Email { get; set; }

        [Required(ErrorMessage = "The password is required")]
        [StringLength(10, MinimumLength = 5)] // 12345 - 1234567890
        public String Password { get; set; }

        [Required(ErrorMessage = "The birthday is required")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "The phone number is required")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone Number")]
        [RegularExpression(@"^([0|\+[0-9]{1,5})?([0-9][0-9]{8})$")]
        public String PhoneNumber { get; set; }

        [DisplayName("Favorite Branch")]
        public Branch FavoriteBranch { get; set; }

        [DisplayName("Orders History")]
        public ICollection<Order> Orders { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public UserType Type { get; set; }
    }
}
