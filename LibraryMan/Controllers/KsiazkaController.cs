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
    public class KsiazkaController : Controller
    {
        private readonly LibraryManContext _context;

        public KsiazkaController(LibraryManContext context)
        {
            _context = context;
        }

        // GET: Ksiazka
        public async Task<IActionResult> Index()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True")
            {
                var libraryManContext = _context.KsiazkaModel.Include(k => k.WydawnictwoModel);
                return View(await libraryManContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Katalog(string BookName, string PublisherName, string Genre, double? MinRating, double? MaxRating, string SortBy)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                var LibraryManContext = _context.KsiazkaModel.Include(p => p.WydawnictwoModel).AsQueryable();

                if (!string.IsNullOrEmpty(BookName))
                {
                    LibraryManContext = LibraryManContext.Where(p => p.BookName == BookName);
                }
                if (!string.IsNullOrEmpty(PublisherName))
                {
                    LibraryManContext = LibraryManContext.Where(p => p.WydawnictwoModel.PublisherName == PublisherName);
                }
                if (!string.IsNullOrEmpty(Genre))
                {
                    LibraryManContext = LibraryManContext.Where(p => p.Genre == Genre);
                }
                if (MinRating.HasValue)
                {
                    LibraryManContext = LibraryManContext.Where(p => p.AverageRating >= MinRating.Value);
                }
                if (MaxRating.HasValue)
                {
                    LibraryManContext = LibraryManContext.Where(p => p.AverageRating <= MaxRating.Value);
                }

                switch (SortBy)
                {
                    case "RatingAsc":
                        LibraryManContext = LibraryManContext.OrderBy(p => p.AverageRating);
                        break;
                    case "RatingDesc":
                        LibraryManContext = LibraryManContext.OrderByDescending(p => p.AverageRating);
                        break;
                }

                ViewBag.BookNames = await _context.KsiazkaModel.Select(p => p.BookName).Distinct().ToListAsync();
                ViewBag.PublisherNames = await _context.WydawnictwoModel.Select(b => b.PublisherName).Distinct().ToListAsync();
                ViewBag.Genres = await _context.KsiazkaModel.Select(p => p.Genre).Distinct().ToListAsync();

                ViewBag.CurrentBookName = BookName;
                ViewBag.CurrentPublisherName = PublisherName;
                ViewBag.CurrentGenre = Genre;
                ViewBag.CurrentMinRating = MinRating;
                ViewBag.CurrentMaxRating = MaxRating;
                ViewBag.CurrentSortBy = SortBy;

                return View(await LibraryManContext.ToListAsync());
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> KatalogRecenzji(string ? BookName)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                ViewData["BookNameRecenzja"] = BookName;
                var LibraryManContext = _context.RecenzjaModel.Include(r => r.KsiazkaModel).Include(r => r.UzytkownikModel).Where(r => r.KsiazkaModel.BookName == BookName);               
                return View(await LibraryManContext.ToListAsync());
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> KatalogWydawnictw(string ? PublisherName)
        {
            if(HttpContext.Session.GetString("IsLoggedIn") == "true") 
            {
                var wydawnictwoModel = _context.WydawnictwoModel.Where(m =>m.PublisherName==PublisherName);
                    
                return View(wydawnictwoModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Ksiazka/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id == null || _context.KsiazkaModel == null)
                {
                    return NotFound();
                }

                var ksiazkaModel = await _context.KsiazkaModel
                    .Include(k => k.WydawnictwoModel)
                    .FirstOrDefaultAsync(m => m.BookName == id);
                if (ksiazkaModel == null)
                {
                    return NotFound();
                }

                return View(ksiazkaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Ksiazka/Create
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                ViewData["PublisherName"] = new SelectList(_context.Set<WydawnictwoModel>(), "PublisherName", "PublisherName");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Ksiazka/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookName,PublisherName,Genre,AverageRating")] KsiazkaModel ksiazkaModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (ModelState.IsValid)
                {
                    _context.Add(ksiazkaModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["PublisherName"] = new SelectList(_context.Set<WydawnictwoModel>(), "PublisherName", "PublisherName", ksiazkaModel.PublisherName);
                return View(ksiazkaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Ksiazka/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True")
            { 
                if (id == null || _context.KsiazkaModel == null)
                {
                    return NotFound();
                }

                var ksiazkaModel = await _context.KsiazkaModel.FindAsync(id);
                if (ksiazkaModel == null)
                {
                    return NotFound();
                }
                ViewData["PublisherName"] = new SelectList(_context.Set<WydawnictwoModel>(), "PublisherName", "PublisherName", ksiazkaModel.PublisherName);
                return View(ksiazkaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Ksiazka/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookName,PublisherName,Genre,AverageRating")] KsiazkaModel ksiazkaModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (id != ksiazkaModel.BookName)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(ksiazkaModel);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KsiazkaModelExists(ksiazkaModel.BookName))
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
                ViewData["PublisherName"] = new SelectList(_context.Set<WydawnictwoModel>(), "PublisherName", "PublisherName", ksiazkaModel.PublisherName);
                return View(ksiazkaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Ksiazka/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True")
            {
                if (id == null || _context.KsiazkaModel == null)
                {
                    return NotFound();
                }

                var ksiazkaModel = await _context.KsiazkaModel
                    .Include(k => k.WydawnictwoModel)
                    .FirstOrDefaultAsync(m => m.BookName == id);
                if (ksiazkaModel == null)
                {
                    return NotFound();
                }

                return View(ksiazkaModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Ksiazka/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (_context.KsiazkaModel == null)
                {
                    return Problem("Entity set 'LibraryManContext.KsiazkaModel'  is null.");
                }
                var ksiazkaModel = await _context.KsiazkaModel.FindAsync(id);
                if (ksiazkaModel != null)
                {
                    _context.KsiazkaModel.Remove(ksiazkaModel);
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
                return RedirectToAction("Index", "Home");
        }

        private bool KsiazkaModelExists(string id)
        {
          return (_context.KsiazkaModel?.Any(e => e.BookName == id)).GetValueOrDefault();
        }
    }
}
