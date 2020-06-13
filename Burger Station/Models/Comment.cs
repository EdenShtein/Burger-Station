using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Item Item { get; set; }

        public DateTime DatePost { get; set; }

        public string TitlePost { get; set; }

        public string BodyPost { get; set; }
    
        public int Score { get; set; }
    }
}
