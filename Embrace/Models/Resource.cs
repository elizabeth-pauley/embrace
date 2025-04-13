using System.ComponentModel.DataAnnotations;

namespace Embrace.Models
{
    public class Resource
    {
        public int Id { get; set; }
        [Display(Name = "Address Id")]
        public int AddressId { get; set; }
        [Display(Name = "Resource Type")]
        public required ResourceType ResourceType { get; set; }
        [Display(Name = "Resource Name")]
        public required string ResourceName { get; set; }
        [Display(Name = "Services Offered")]
        public ICollection<ResourceServiceCategories> ServiceCategories { get; set; } = new List<ResourceServiceCategories>();
        public string? LogoImage { get; set; }
        public string? LocationImage { get; set; }
        public required string Description { get; set; }
        [Display(Name = "Phone Number")]
        public int? PhoneNumber { get; set; }
        [Display(Name = "Website Link")]
        public string? WebsiteUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
    }
}
