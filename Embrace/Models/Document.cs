using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Embrace.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Untitled Document";
        public string OriginalLanguage { get; set; }
        public string OriginalDataPath { get; set; }
        public string TranslatedDataPath { get; set; }
        public string TargetLanguage { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
