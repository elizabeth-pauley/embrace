using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string Address { get; set; } = ""; // TO-DO: change to address class
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
