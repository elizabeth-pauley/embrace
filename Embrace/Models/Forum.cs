using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    // Contains a collection of related posts
    public class Forum
    {
        // reference code: https://www.youtube.com/watch?v=9HNy4ZVG9IQ&list=PL3_YUnRN3Uhiz2HomrXKcaEW6b3pDhKTX&index=4
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public ICollection<DiscussionBoard> DiscussionBoards { get; set; } = new List<DiscussionBoard>();
    }
}
