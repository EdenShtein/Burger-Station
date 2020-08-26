using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class ItemComment
    {
        public int ItemId { get; set; }
        public int CommentId { get; set; }
        public string ItemName { get; set; }
        public string PostedBy { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public DateTime PostDate { get; set; }
    }
}
