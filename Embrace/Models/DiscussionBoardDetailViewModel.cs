using Embrace.Controllers;
using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class DiscussionBoardDetailViewModel
    {
        public DiscussionBoard DiscussionBoard { get; set; }

        public List<Comment>? Comments { get; set; } = new List<Comment>();

        [Required(ErrorMessage = "Comment cannot be empty.")]
        public string NewCommentContent { get; set; }
    }
}
