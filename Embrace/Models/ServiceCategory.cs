using System.ComponentModel;

namespace Embrace.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<ResourceServiceCategories> Resources { get; set; }
    }
}
