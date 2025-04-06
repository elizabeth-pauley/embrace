using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Embrace.Models
{
    public class CreateDocumentViewModel
    {
        [StringLength(100)]
        public required string Title { get; set; }

        [StringLength(3)]
        public required string OriginalLanguage { get; set; }

        [StringLength(3)]
        public required string TargetLanguage { get; set; }
    
        public IFormFile DocumentFile { get; set; }
    }
}
