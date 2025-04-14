using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Embrace.Data;
using Embrace.Models;

namespace Embrace.Controllers
{
    public class DiscussionBoardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscussionBoardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DiscussionBoards
        public async Task<IActionResult> Index(DiscussionType? discussionType, string? searchString)
        {
            var discussionBoardQuery = _context.DiscussionBoards.Include(d => d.User).AsQueryable();

            // Filter by search term
            if (!string.IsNullOrEmpty(searchString))
            {
                discussionBoardQuery = discussionBoardQuery.Where(x => x.Title!.ToUpper().Contains(searchString.ToUpper()) ||
                                                                        x.Description!.ToUpper().Contains(searchString.ToUpper()));
            }

            // Filter by discussion type
            if (discussionType != null)
            {
                discussionBoardQuery = discussionBoardQuery.Where(x => x.DiscussionType == discussionType);
            }

            var discussionBoards = await discussionBoardQuery.ToListAsync();

            // Use LINQ to get list of discussion types + tags
            IQueryable<DiscussionType> discussionTypeQuery = from r in _context.DiscussionBoards
                                                         orderby r.DiscussionType
                                                         select r.DiscussionType;

            var discussionBoardVM = new DiscussionBoardViewModel
            {
                DiscussionTypes = new SelectList(await discussionTypeQuery.Distinct().ToListAsync()),
                DiscussionBoards = discussionBoards
            };

            return View(discussionBoardVM);
        }

        // GET: DiscussionBoards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussionBoard = await _context.DiscussionBoards
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discussionBoard == null)
            {
                return NotFound();
            }

            return View(discussionBoard);
        }

        // GET: DiscussionBoards/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: DiscussionBoards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DiscussionType,Title,Description,CreatedOn,UserId")] DiscussionBoard discussionBoard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(discussionBoard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", discussionBoard.User.Id);
            return View(discussionBoard);
        }

        // GET: DiscussionBoards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussionBoard = await _context.DiscussionBoards.FindAsync(id);
            if (discussionBoard == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", discussionBoard.User.Id);
            return View(discussionBoard);
        }

        // POST: DiscussionBoards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DiscussionType,Title,Description,CreatedOn,UserId")] DiscussionBoard discussionBoard)
        {
            if (id != discussionBoard.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussionBoard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionBoardExists(discussionBoard.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", discussionBoard.User.Id);
            return View(discussionBoard);
        }

        // GET: DiscussionBoards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussionBoard = await _context.DiscussionBoards
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discussionBoard == null)
            {
                return NotFound();
            }

            return View(discussionBoard);
        }

        // POST: DiscussionBoards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussionBoard = await _context.DiscussionBoards.FindAsync(id);
            if (discussionBoard != null)
            {
                _context.DiscussionBoards.Remove(discussionBoard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscussionBoardExists(int id)
        {
            return _context.DiscussionBoards.Any(e => e.Id == id);
        }
    }
}
