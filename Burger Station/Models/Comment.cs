using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Burger_Station.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [DisplayName("Title of the Post")]
        [StringLength(70, MinimumLength = 2)]
        public String PostTitle { get; set; }

        [DisplayName("Post")]
        [StringLength(400, MinimumLength = 2)]
        public String PostBody { get; set; }

        [DisplayName("Date of Post")]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [DisplayName("Author")]
        public String PostedBy { get; set; }

        public Item Item { get; set; }
    }
}
