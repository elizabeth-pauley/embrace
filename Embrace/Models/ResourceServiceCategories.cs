namespace Embrace.Models
{
    public class ResourceServiceCategories
    {
        public int ResourceId { get; set; }
        public required Resource Resource { get; set; }

        public int ServiceCategoryId { get; set; }
        public required ServiceCategory ServiceCategory { get; set; }
    }

}
