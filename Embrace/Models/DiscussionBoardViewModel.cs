using Microsoft.AspNetCore.Mvc.Rendering;

namespace Embrace.Models
{
    public class DiscussionBoardViewModel
    {
        public List<DiscussionBoard>? DiscussionBoards { get; set; }
        public DiscussionType? DiscussionType { get; set; }
        public SelectList? DiscussionTypes { get; set; }
        public string? SearchString { get; set; }
    }
}
