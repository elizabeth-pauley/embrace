using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class CreateDiscussionBoardViewModel
    {
        [Required]
        public DiscussionType DiscussionType { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }

        public SelectList? DiscussionTypes { get; set; }
    }
}
