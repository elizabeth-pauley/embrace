using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Embrace.Models
{
    public class ResourceViewModel
    {
        public List<Resource>? Resources { get; set; }
        public ResourceType? ResourceType { get; set; }
        public int? ServiceCategoryId { get; set; }
        public SelectList? ResourceTypes { get; set; }
        public SelectList? ServiceCategories { get; set; }
        public string? SearchString { get; set; }
    }
}
