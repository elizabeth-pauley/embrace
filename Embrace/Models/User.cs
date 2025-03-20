using Microsoft.AspNetCore.Identity;

namespace Embrace.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; } = ""; // TO-DO: change to address class
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
