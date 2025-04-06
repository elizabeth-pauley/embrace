using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class DiscussionBoard
    {
        public int Id { get; set; }
        public required DiscussionType DiscussionType { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public required string UserId { get; set; }
        public required User User { get; set; }
    }
}
