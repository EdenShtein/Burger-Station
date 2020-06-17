using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Burger_Station.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The author is required")]
        [DisplayName("Author")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        public User PostBy { get; set; }

        [Required(ErrorMessage = "The item is required")]
        public Item Item { get; set; }

        [Required(ErrorMessage = "The date of the post is required")]
        [DisplayName("Date of Post")]
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        [Required(ErrorMessage = "The title of the post is required")]
        [DisplayName("Title of the Post")]
        [RegularExpression(@"^[A-Za-z\s]*$")]
        [StringLength(70, MinimumLength = 2)]
        public String PostTitle { get; set; }

        [Required(ErrorMessage = "The body of the post is required")]
        [DisplayName("Post")]
        [RegularExpression(@"^[A-Za-z0-9\s]*$")]
        [StringLength(250, MinimumLength = 2)]
        public String PostBody { get; set; }
    }
}