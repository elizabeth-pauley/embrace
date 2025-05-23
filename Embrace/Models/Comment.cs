﻿using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public required string Content { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual DiscussionBoard DiscussionBoard { get; set; }

        [Required]
        public virtual string UserId { get; set; }
        [Required]
        public virtual int DiscussionBoardId { get; set; }
    }
}
