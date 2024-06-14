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
    public class WydawnictwoController : Controller
    {
        private readonly LibraryManContext _context;

        public WydawnictwoController(LibraryManContext context)
        {
            _context = context;
        }

        // GET: Wydawnictwo
        public async Task<IActionResult> Index()
        {
              return _context.WydawnictwoModel != null ? 
                          View(await _context.WydawnictwoModel.ToListAsync()) :
                          Problem("Entity set 'LibraryManContext.WydawnictwoModel'  is null.");
        }

        // GET: Wydawnictwo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.WydawnictwoModel == null)
            {
                return NotFound();
            }

            var wydawnictwoModel = await _context.WydawnictwoModel
                .FirstOrDefaultAsync(m => m.PublisherName == id);
            if (wydawnictwoModel == null)
            {
                return NotFound();
            }

            return View(wydawnictwoModel);
        }

        // GET: Wydawnictwo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wydawnictwo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublisherName,City,Country,Founded,Description")] WydawnictwoModel wydawnictwoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wydawnictwoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wydawnictwoModel);
        }

        // GET: Wydawnictwo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.WydawnictwoModel == null)
            {
                return NotFound();
            }

            var wydawnictwoModel = await _context.WydawnictwoModel.FindAsync(id);
            if (wydawnictwoModel == null)
            {
                return NotFound();
            }
            return View(wydawnictwoModel);
        }

        // POST: Wydawnictwo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PublisherName,City,Country,Founded,Description")] WydawnictwoModel wydawnictwoModel)
        {
            if (id != wydawnictwoModel.PublisherName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wydawnictwoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WydawnictwoModelExists(wydawnictwoModel.PublisherName))
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
            return View(wydawnictwoModel);
        }

        // GET: Wydawnictwo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.WydawnictwoModel == null)
            {
                return NotFound();
            }

            var wydawnictwoModel = await _context.WydawnictwoModel
                .FirstOrDefaultAsync(m => m.PublisherName == id);
            if (wydawnictwoModel == null)
            {
                return NotFound();
            }

            return View(wydawnictwoModel);
        }

        // POST: Wydawnictwo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.WydawnictwoModel == null)
            {
                return Problem("Entity set 'LibraryManContext.WydawnictwoModel'  is null.");
            }
            var wydawnictwoModel = await _context.WydawnictwoModel.FindAsync(id);
            if (wydawnictwoModel != null)
            {
                _context.WydawnictwoModel.Remove(wydawnictwoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WydawnictwoModelExists(string id)
        {
          return (_context.WydawnictwoModel?.Any(e => e.PublisherName == id)).GetValueOrDefault();
        }
    }
}
