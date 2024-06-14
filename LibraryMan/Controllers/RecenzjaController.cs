using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryMan.Data;
using LibraryMan.Models;

namespace LibraryMan.Controllers
{
    public class RecenzjaController : Controller
    {
        private readonly LibraryManContext _context;

        public RecenzjaController(LibraryManContext context)
        {
            _context = context;
        }

        // GET: Recenzja
        public async Task<IActionResult> Index()
        {
            var libraryManContext = _context.RecenzjaModel.Include(r => r.KsiazkaModel).Include(r => r.UzytkownikModel);
            return View(await libraryManContext.ToListAsync());
        }

        // GET: Recenzja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RecenzjaModel == null)
            {
                return NotFound();
            }

            var recenzjaModel = await _context.RecenzjaModel
                .Include(r => r.KsiazkaModel)
                .Include(r => r.UzytkownikModel)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (recenzjaModel == null)
            {
                return NotFound();
            }

            return View(recenzjaModel);
        }

        // GET: Recenzja/Create
        public IActionResult Create(string ? BookName)
        {
            ViewData["BookName"] = new SelectList(_context.KsiazkaModel, "BookName", "BookName");
            ViewData["UserID"] = new SelectList(_context.Set<UzytkownikModel>(), "UserID", "Email");
            ViewData["UserIDRecenzja"] = _context.UzytkownikModel.FirstOrDefault(m => m.Email == HttpContext.Session.GetString("email")).UserID;                
            ViewData["BookNameRecenzja"] = BookName;
            return View();
        }

        // POST: Recenzja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReviewID,UserID,BookName,Rating,Comment,ReviewDate")] RecenzjaModel recenzjaModel)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                if (ModelState.IsValid)
                {                                    
                    recenzjaModel.ReviewDate = DateTime.Now;
                    _context.Add(recenzjaModel);
                    await _context.SaveChangesAsync();

                    var averageRating = _context.RecenzjaModel
                    .Where(r => r.BookName == recenzjaModel.BookName)
                    .Average(r => r.Rating);

                    var book = _context.KsiazkaModel.FirstOrDefault(p => p.BookName == recenzjaModel.BookName);
                    if (book != null)
                    {
                        book.AverageRating = averageRating;
                        _context.Update(book);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("KatalogRecenzji","Ksiazka",new { bookName = recenzjaModel.BookName });
                }
                ViewData["BookName"] = new SelectList(_context.KsiazkaModel, "BookName", "BookName", recenzjaModel.BookName);
                ViewData["UserID"] = new SelectList(_context.UzytkownikModel, "UserID", "Email", recenzjaModel.UserID);
                return View(recenzjaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Recenzja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RecenzjaModel == null)
            {
                return NotFound();
            }

            var recenzjaModel = await _context.RecenzjaModel.FindAsync(id);
            if (recenzjaModel == null)
            {
                return NotFound();
            }
            ViewData["BookName"] = new SelectList(_context.KsiazkaModel, "BookName", "BookName", recenzjaModel.BookName);
            ViewData["UserID"] = new SelectList(_context.Set<UzytkownikModel>(), "UserID", "Email", recenzjaModel.UserID);
            return View(recenzjaModel);
        }

        // POST: Recenzja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewID,UserID,BookName,Rating,Comment,ReviewDate")] RecenzjaModel recenzjaModel)
        {
            if (id != recenzjaModel.ReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recenzjaModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecenzjaModelExists(recenzjaModel.ReviewID))
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
            ViewData["BookName"] = new SelectList(_context.KsiazkaModel, "BookName", "BookName", recenzjaModel.BookName);
            ViewData["UserID"] = new SelectList(_context.Set<UzytkownikModel>(), "UserID", "Email", recenzjaModel.UserID);
            return View(recenzjaModel);
        }

        // GET: Recenzja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RecenzjaModel == null)
            {
                return NotFound();
            }

            var recenzjaModel = await _context.RecenzjaModel
                .Include(r => r.KsiazkaModel)
                .Include(r => r.UzytkownikModel)
                .FirstOrDefaultAsync(m => m.ReviewID == id);
            if (recenzjaModel == null)
            {
                return NotFound();
            }

            return View(recenzjaModel);
        }

        // POST: Recenzja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RecenzjaModel == null)
            {
                return Problem("Entity set 'LibraryManContext.RecenzjaModel'  is null.");
            }
            var recenzjaModel = await _context.RecenzjaModel.FindAsync(id);
            if (recenzjaModel != null)
            {
                _context.RecenzjaModel.Remove(recenzjaModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecenzjaModelExists(int id)
        {
          return (_context.RecenzjaModel?.Any(e => e.ReviewID == id)).GetValueOrDefault();
        }
    }
}
