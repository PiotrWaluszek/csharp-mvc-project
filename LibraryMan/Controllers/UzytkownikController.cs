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
    public class UzytkownikController : Controller
    {
        private readonly LibraryManContext _context;

        public UzytkownikController(LibraryManContext context)
        {
            _context = context;
        }

        // GET: Uzytkownik
        public async Task<IActionResult> Index()
        {
              return _context.UzytkownikModel != null ? 
                          View(await _context.UzytkownikModel.ToListAsync()) :
                          Problem("Entity set 'LibraryManContext.UzytkownikModel'  is null.");
        }

        // GET: Uzytkownik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UzytkownikModel == null)
            {
                return NotFound();
            }

            var uzytkownikModel = await _context.UzytkownikModel
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (uzytkownikModel == null)
            {
                return NotFound();
            }

            return View(uzytkownikModel);
        }

        // GET: Uzytkownik/Create
        public IActionResult Create()
        {
            return View();
        }

// POST: UzytkownikModel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Email,Password,IsAdmin, Token")] UzytkownikModel uzytkownikModel)
        {
            if(HttpContext.Session.GetString("IsAdmin") == "True") 
            {
                if (ModelState.IsValid)
                {
                    uzytkownikModel.Password = LibraryMan.Commons.Hash.CalculateMD5Hash(uzytkownikModel.Password);
                    if(uzytkownikModel.IsAdmin){
                        uzytkownikModel.Token = LibraryMan.Commons.Tokens.GenerateToken();
                    }
                    _context.Add(uzytkownikModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine("\n");
                            Console.WriteLine(error.ErrorMessage);
                            Console.WriteLine("\n");
                        }
                    }
                }
                return View(uzytkownikModel);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Uzytkownik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UzytkownikModel == null)
            {
                return NotFound();
            }

            var uzytkownikModel = await _context.UzytkownikModel.FindAsync(id);
            if (uzytkownikModel == null)
            {
                return NotFound();
            }
            return View(uzytkownikModel);
        }

        // POST: Uzytkownik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Email,Password,IsAdmin,Token")] UzytkownikModel uzytkownikModel)
        {
            if (id != uzytkownikModel.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uzytkownikModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UzytkownikModelExists(uzytkownikModel.UserID))
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
            return View(uzytkownikModel);
        }

        // GET: Uzytkownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UzytkownikModel == null)
            {
                return NotFound();
            }

            var uzytkownikModel = await _context.UzytkownikModel
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (uzytkownikModel == null)
            {
                return NotFound();
            }

            return View(uzytkownikModel);
        }

        // POST: Uzytkownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UzytkownikModel == null)
            {
                return Problem("Entity set 'LibraryManContext.UzytkownikModel'  is null.");
            }
            var uzytkownikModel = await _context.UzytkownikModel.FindAsync(id);
            if (uzytkownikModel != null)
            {
                _context.UzytkownikModel.Remove(uzytkownikModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UzytkownikModelExists(int id)
        {
          return (_context.UzytkownikModel?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}
