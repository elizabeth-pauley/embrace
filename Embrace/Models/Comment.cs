using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public required int DiscussionBoardId { get; set; }
        public required DiscussionBoard DiscussionBoard { get; set; }
    }
}
