using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }

        public ICollection<ResourceServiceCategories> Resources { get; set; }
    }
}
