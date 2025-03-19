using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public required ResourceType ResourceType { get; set; }
        public required string ResourceName { get; set; }
        public ICollection<ResourceServiceCategories> ServiceCategories { get; set; } = new List<ResourceServiceCategories>();
        public string? LogoImage { get; set; }
        public string? LocationImage { get; set; }
        public required string Description { get; set; }
        public int? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
    }
}
