using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public User PostBy { get; set; }

        public Item Item { get; set; }

        public DateTime PostDate { get; set; }

        public String PostTitle { get; set; }

        public String PostBody { get; set; }
    
        public int ReviewScore { get; set; }
    }
}
