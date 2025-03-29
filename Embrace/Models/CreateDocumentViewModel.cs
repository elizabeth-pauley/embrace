using System.ComponentModel.DataAnnotations;
namespace Embrace.Models
{
    public class CreateDocumentViewModel
    {
        [StringLength(100)]
        public required string Title { get; set; }



    }
}
