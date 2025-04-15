using Microsoft.AspNetCore.Mvc.Rendering;

namespace Embrace.Models
{
    public class CreateDiscussionBoardViewModel
    {
        public required DiscussionType DiscussionType { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public SelectList? DiscussionTypes { get; set; }
    }
}
