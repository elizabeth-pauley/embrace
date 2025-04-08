using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Embrace.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Untitled Document";
        public string UploadedFileName { get; set; }
        public string TranslatedFileName { get; set; }
        public string UploadedFilePath { get; set; }
        public string TranslatedFilePath { get; set; }
        public string OriginalLanguage { get; set; }
        public string TargetLanguage { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public required string UserId { get; set; }
        public required User User { get; set; }
    }
}
