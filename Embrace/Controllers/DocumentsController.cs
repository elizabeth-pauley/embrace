using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Embrace.Data;
using Embrace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using VSLangProj;
using Microsoft.VisualStudio.OLE.Interop;

namespace Embrace.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _documentsFolderPath;

        public DocumentsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _documentsFolderPath = Path.Combine(_hostEnvironment.WebRootPath, "UploadedDocuments");

            // Ensure the images folder exists
            if (!Directory.Exists(_documentsFolderPath))
            {
                Directory.CreateDirectory(_documentsFolderPath);
            }
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Documents.Include(d => d.User);
            var documents = await applicationDbContext.AsQueryable().ToListAsync();

            var documentVM = new DocumentViewModel
            {
                Documents = documents
            };

            return View(documentVM);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Create([Bind("Id,Title,OriginalLanguage,OriginalDataPath,TranslatedDataPath,TargetLanguage,CreatedOn,UserId")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", document.UserId);
            return View(document);
        }*/
        public async Task<IActionResult> Create(CreateDocumentViewModel vm)
        {
            // If the submitted model is invalid, re-display the form.
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Get currently logged in user
            var userId = _userManager.GetUserId(User);

            // Create a new Document entity from the view model data.
            var document = new Document
            {
                Title = vm.Title,
                OriginalLanguage = vm.OriginalLanguage,
                TargetLanguage = vm.TargetLanguage,
                UserId = userId!,
                User = _context.Users.FindAsync(userId!).Result!,
                CreatedOn = DateTime.Now
            };

            // Process document upload using buffering approach
            if (vm.DocumentFile != null && vm.DocumentFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await vm.DocumentFile.CopyToAsync(memoryStream); // Read file into memory
                var fileBytes = memoryStream.ToArray(); // Convert the buffered stream to a byte array
                // Optionally store the file bytes in the database (not recommended in production)
                document.DocumentData = fileBytes;
                // Generate a unique file name to avoid collisions and assign it to the product.
                var uniqueFileName = $"{Guid.NewGuid().ToString()}_{document.Title.ToString().Replace(" ", "")}";
                document.FileName = uniqueFileName;
                // Write the buffered file bytes to the specified location on disk.
                var filePath = Path.Combine(_documentsFolderPath, uniqueFileName);
                document.FilePath = filePath;
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);
            }
            // Save the document to the database
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            // Redirect to the Index view after successfully creating the product.
            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", document.UserId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,OriginalLanguage,OriginalDataPath,TranslatedDataPath,TargetLanguage,CreatedOn,UserId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
