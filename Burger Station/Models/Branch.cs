using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String Address { get; set; }

        public String PhoneNumber { get; set; }
        
        public ICollection<User> FavoriteUsers { get; set; }
    }
}
