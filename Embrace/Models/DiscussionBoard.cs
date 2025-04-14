using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class DiscussionBoard
    {
        // reference code: https://www.youtube.com/watch?v=9HNy4ZVG9IQ&list=PL3_YUnRN3Uhiz2HomrXKcaEW6b3pDhKTX&index=4
        public int Id { get; set; }
        public required DiscussionType DiscussionType { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Forum Forum { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
