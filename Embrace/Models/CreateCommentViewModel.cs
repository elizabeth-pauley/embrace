using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class CreateCommentViewModel
    {
        public int DiscussionBoardId { get; set; }

        [Required(ErrorMessage = "Comment cannot be empty.")]
        public string NewCommentContent { get; set; }
    }
}
