using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Embrace.Models;
using Microsoft.EntityFrameworkCore;

namespace Embrace.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Seed ServiceCategories if not already present
            if (!context.ServiceCategories.Any())
            {
                var serviceCategories = new List<ServiceCategory>
                {
                    new ServiceCategory { Name = "Healthcare" },
                    new ServiceCategory { Name = "Childcare" },
                    new ServiceCategory { Name = "English Education" },
                    new ServiceCategory { Name = "Translation Assistance" },
                    new ServiceCategory { Name = "Legal Aid"},
                    new ServiceCategory { Name = "Citizenship"},
                    new ServiceCategory { Name = "Managing Finances/Financial Aid"},
                    new ServiceCategory { Name = "Driver's License"},
                    new ServiceCategory { Name = "Housing"},
                    new ServiceCategory { Name = "Employment"},
                    new ServiceCategory { Name = "Social Engagement"}

                };

                context.ServiceCategories.AddRange(serviceCategories);
                await context.SaveChangesAsync();
            }

            // Seed Resources after ServiceCategories are seeded
            if (!context.Resources.Any())
            {
                await CreateTestResources(context);
            }
        }

        public static async Task CreateTestResources(ApplicationDbContext context)
        {
            // Seed Service Categories
            var serviceCategories = await context.ServiceCategories.ToListAsync();

            // Create Resources
            var resources = new List<Resource>
                {
                    new Resource
                    {
                        ResourceType = ResourceType.Local,
                        ResourceName = "Bienvenidos",
                        Description = "Bienvenidos illuminates pathways to self empowerment and belonging for the Spanish-speaking community",
                        LogoImage = "https://bienvenidosgv.org/hs-fs/hubfs/Untitled%2Bdesign.png?width=450&height=180&name=Untitled%2Bdesign.png",
                        WebsiteUrl = "https://bienvenidosgv.org"
                    },
                    new Resource
                    {
                        ResourceType = ResourceType.Local,
                        ResourceName = "Proyecto Salud",
                        Description = "An interdisciplinary team of researchers, students, and community members, seeks to reduce health disparities, improve health outcomes, and make a significant difference in the health of the Latino immigrant population in Montana",
                        LogoImage = "https://www.montana.edu/nursing/salud/images/New%20Logo%20from%20Cass.png",
                        WebsiteUrl = "https://www.montana.edu/nursing/salud/"
                    },                 
                    new Resource
                    {
                        ResourceType = ResourceType.General,
                        ResourceName = "Immigrant Legal Resource Center",
                        Description = "Providing legal resources and education for immigrants",
                        LogoImage = "https://i.pinimg.com/originals/b4/44/fa/b444fa5d1d9c1ad1442078458c3cec45.png",
                        WebsiteUrl = "https://ilrc.org"
                    },
                    new Resource
                    {
                        ResourceType = ResourceType.General,
                        ResourceName = "U.S. Citizenship and Immigration Services",
                        Description = "USCIS offers helpful information about education, child care, employment, what to do in case of an emergency, and a number of popular topics that will help new immigrants settle in the U.S.",
                        LogoImage = "https://www.uscis.gov/sites/default/files/USCIS_Signature_Preferred_FC.png",
                        WebsiteUrl = "https://www.uscis.gov/citizenship/civic-integration/settling-in-the-us"
                    }
                };

            context.Resources.AddRange(resources);
            await context.SaveChangesAsync(); // Save resources to generate IDs

            var resourceServiceCategories = new List<ResourceServiceCategories>
                {
                    new ResourceServiceCategories
                    {
                        Resource = resources[0],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Social Engagement")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[0],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Translation Assistance")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[1],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Healthcare")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[2],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Legal Aid")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[2],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Citizenship")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[3],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "English Education")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[3],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Childcare")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[3],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Managing Finances/Financial Aid")
                    },
                    new ResourceServiceCategories
                    {
                        Resource = resources[3],
                        ServiceCategory = serviceCategories.First(sc => sc.Name == "Healthcare")
                    }
                };

            context.ResourceServiceCategories.AddRange(resourceServiceCategories);
            await context.SaveChangesAsync();
        }

    }
}
