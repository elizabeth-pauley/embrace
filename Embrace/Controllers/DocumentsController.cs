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
using Google.Cloud.Translate.V3;
using Google.Api.Gax.ResourceNames;
using Microsoft.CodeAnalysis;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Google;
using System.Security.AccessControl;
using NuGet.Protocol;

namespace Embrace.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string _uploadedDocumentsBucket;
        private readonly string _translatedDocumentsBucket;
        private readonly IConfiguration _configuration;
        private readonly TranslationServiceClient _client;

        public DocumentsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<User> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _uploadedDocumentsBucket = "embrace-uploaded-documents";
            _translatedDocumentsBucket = "embrace-translated-documents";

            _configuration = configuration;

            var serviceAccountKeyPath = _configuration["GoogleCloud:ServiceAccountKey"];
            if (!string.IsNullOrEmpty(serviceAccountKeyPath)) // add later: && File.Exists(serviceAccountKeyPath) - was throwing an error for some reason
            {
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", serviceAccountKeyPath);
            }

            _client = TranslationServiceClient.Create();
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var applicationDbContext = _context.Documents.Include(d => d.User)
                                .Where(x => x.UserId == userId);
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
                .Include(d => d.UserId)
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
        public async Task<IActionResult> Create(CreateDocumentViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // Get currently logged in user
            var userId = _userManager.GetUserId(User);

            var document = new Models.Document
            {
                Title = vm.Title,
                OriginalLanguage = vm.OriginalLanguage,
                TargetLanguage = vm.TargetLanguage,
                UserId = userId,
            };

            if (vm.DocumentFile != null && vm.DocumentFile.Length > 0)
            {
                // Generate a unique file name and ensure it has a .pdf extension.
                var uniqueFileName = $"{Guid.NewGuid()}_{document.Title.Replace(" ", "")}";
                var gcsUploadedFileUri = $"gs://{_uploadedDocumentsBucket}/{uniqueFileName}.pdf";
                document.UploadedFileName = uniqueFileName + ".pdf";
                document.UploadedFilePath = gcsUploadedFileUri;
                document.TranslatedFilePath = "";

                // Build the output URI prefix for translated documents
                var gcsTranslatedFileUriPrefix = $"gs://{_translatedDocumentsBucket}/";

                // Upload the file to Google Cloud Storage using the StorageClient
                var storageClient = StorageClient.Create();
                using (var stream = vm.DocumentFile.OpenReadStream())
                {
                    var uploadObject = await storageClient.UploadObjectAsync(
                        _uploadedDocumentsBucket,
                        uniqueFileName + ".pdf",
                        "application/pdf",
                        stream
                    );

                    if (uploadObject != null)
                    {
                        document.UploadedFilePath = gcsUploadedFileUri;
                    }
                    else
                    {
                        Console.WriteLine("Upload Failed.");
                        return View(vm);
                    }
                }

                // TRANSLATE DOCUMENT
                var request = new TranslateDocumentRequest
                {
                    Parent = $"projects/embrace-456119/locations/global",
                    SourceLanguageCode = vm.OriginalLanguage,
                    TargetLanguageCode = vm.TargetLanguage,
                    DocumentInputConfig = new DocumentInputConfig
                    {
                        GcsSource = new GcsSource
                        {
                            InputUri = gcsUploadedFileUri
                        },
                        MimeType = "application/pdf"
                    },
                    DocumentOutputConfig = new DocumentOutputConfig
                    {
                        GcsDestination = new GcsDestination
                        {
                            // Provide the output destination; OutputUriPrefix means the file will get saved with a generated name
                            OutputUriPrefix = gcsTranslatedFileUriPrefix,
                        },
                        MimeType = "application/pdf"
                    },
                    IsTranslateNativePdfOnly = false
                };
                var response = await _client.TranslateDocumentAsync(request);
                Console.WriteLine($"Document translated. Response: {response}");

                // save output URI to access document later
                var prefix = "embrace-uploaded-documents_" + uniqueFileName;
                var objects = storageClient.ListObjects(_translatedDocumentsBucket, prefix);
                var finalObject = objects.FirstOrDefault();
                document.TranslatedFileName = finalObject.Name;

                if (finalObject != null)
                {
                    string finalOutputUri = $"gs://{_translatedDocumentsBucket}/{finalObject.Name}";
                    Console.WriteLine($"Final translated document URI: {finalOutputUri}");
                    document.TranslatedFilePath = finalOutputUri;
                }
                else
                {
                    Console.WriteLine("Translated file not found.");
                }
            }

            // Save the document record to the database
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,OriginalLanguage,OriginalDataPath,TranslatedDataPath,TargetLanguage,CreatedOn,UserId")] Models.Document document)
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

        // GET: Documents/ViewDocument/5
        public async Task<IActionResult> ViewDocument(int? id)
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

            if (string.IsNullOrEmpty(document.TranslatedFilePath))
            {
                return NotFound("No translated file path is available.");
            }

            try
            {
                var storageClient = StorageClient.Create();

                // Download the document into a MemoryStream.
                var memoryStream = new MemoryStream();
                await storageClient.DownloadObjectAsync(_translatedDocumentsBucket, document.TranslatedFileName, memoryStream);
                memoryStream.Position = 0;

                string contentType = "application/pdf";

                return File(memoryStream, contentType);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error view or message.
                Console.WriteLine($"Error downloading file from GCS: {ex.Message}");
                throw;
            }
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}
