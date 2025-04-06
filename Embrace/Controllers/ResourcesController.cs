using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Embrace.Data;
using Embrace.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Microsoft.IdentityModel.Tokens;

namespace Embrace.Controllers
{
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resources
        public async Task<IActionResult> Index(ResourceType? resourceType, int? serviceCategoryId, string? searchString)
        {
            var resourcesQuery = _context.Resources
                                          .Include(r => r.ServiceCategories)
                                          .ThenInclude(rc => rc.ServiceCategory)                    
                                          .AsQueryable();

            // Filter by search term
            if (!string.IsNullOrEmpty(searchString))
            {
                resourcesQuery = resourcesQuery.Where(x => x.ResourceName!.ToUpper().Contains(searchString.ToUpper()));
            }

            // Filter by resource type
            if (resourceType != null)
            {
                resourcesQuery = resourcesQuery.Where(x => x.ResourceType == resourceType);
            }

            // Filter by service category
            if (serviceCategoryId.HasValue)
            {
                resourcesQuery = resourcesQuery.Where(x => x.ServiceCategories
                    .Any(sc => sc.ServiceCategory.Id == serviceCategoryId));
            }

            var resources = await resourcesQuery.ToListAsync();

            // Use LINQ to get list of resource types + tags
            IQueryable<ResourceType> resourceTypeQuery = from r in _context.Resources
                                                         orderby r.ResourceType
                                                         select r.ResourceType;

            IOrderedQueryable<ServiceCategory> resourceServiceCategoryQuery = from sc in _context.ServiceCategories
                                                                       orderby sc.Name
                                                                       select sc;

            var resourceVM = new ResourceViewModel
            {
                ResourceTypes = new SelectList(await resourceTypeQuery.Distinct().ToListAsync()),
                ServiceCategories = new SelectList(await resourceServiceCategoryQuery.Distinct().ToListAsync(), "Id", "Name"),
                Resources = resources
            };

            return View(resourceVM);
        }

        // GET: Resources1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // GET: Resources1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Resources1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AddressId,ResourceType,ResourceName,ResourceTags,LogoImage,LocationImage,Description,PhoneNumber,WebsiteUrl,CreatedOn")] Resource resource, List<string> serviceCategoryNames)
        {
            // Ensure all the service categories exist in the database
            foreach (var categoryName in serviceCategoryNames)
            {
                var serviceCategory = await _context.ServiceCategories
                    .FirstOrDefaultAsync(sc => sc.Name == categoryName);

                if (serviceCategory != null)
                {
                    // Add the association between the resource and the service category
                    resource.ServiceCategories.Add(new ResourceServiceCategories
                    {
                        Resource = resource,
                        ServiceCategory = serviceCategory
                    });
                }
            }
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Resources1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        // GET: Resources1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resource = await _context.Resources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        // POST: Resources1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource != null)
            {
                _context.Resources.Remove(resource);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
