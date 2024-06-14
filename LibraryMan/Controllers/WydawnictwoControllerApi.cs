using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryMan.Data;
using LibraryMan.Models;

namespace LibraryMan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WydawnictwoControllerApi : ControllerBase
    {
        private readonly LibraryManContext _context;

        public WydawnictwoControllerApi(LibraryManContext context)
        {
            _context = context;
        }

        // GET: api/WydawnictwoControllerApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WydawnictwoModel>>> GetWydawnictwoModel()
        {
          var accessToken = HttpContext.Request.Headers["Authorization"];
          var email = HttpContext.Request.Headers["Email"];
          var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
          if (userContext && accessToken.ToString()!="0")
          {
                if (_context.WydawnictwoModel == null)
                {
                    return NotFound();
                }
                return await _context.WydawnictwoModel.ToListAsync();
            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
        }

        // GET: api/WydawnictwoControllerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WydawnictwoModel>> GetWydawnictwoModel(string id)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"];
            var email = HttpContext.Request.Headers["Email"];
            var userContext = _context.UzytkownikModel.Where(p=>p.Email==email.ToString()).Where(m => m.Token == accessToken.ToString()).Any();
            if (userContext && accessToken.ToString()!="0")
            {
            
          if (_context.WydawnictwoModel == null)
          {
              return NotFound();
          }
            var wydawnictwoModel = await _context.WydawnictwoModel.FindAsync(id);

            if (wydawnictwoModel == null)
            {
                return NotFound();
            }
            

            return wydawnictwoModel;
            }
            else
            {
                return Problem("Autoryzacja się nie powiodła.");
            }
        }

        // PUT: api/WydawnictwoControllerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWydawnictwoModel(string id, WydawnictwoModel wydawnictwoModel)
        {
            if (id != wydawnictwoModel.PublisherName)
            {
                return BadRequest();
            }

            _context.Entry(wydawnictwoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WydawnictwoModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WydawnictwoControllerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WydawnictwoModel>> PostWydawnictwoModel(WydawnictwoModel wydawnictwoModel)
        {
          if (_context.WydawnictwoModel == null)
          {
              return Problem("Entity set 'LibraryManContext.WydawnictwoModel'  is null.");
          }
            _context.WydawnictwoModel.Add(wydawnictwoModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WydawnictwoModelExists(wydawnictwoModel.PublisherName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetWydawnictwoModel", new { id = wydawnictwoModel.PublisherName }, wydawnictwoModel);
        }

        // DELETE: api/WydawnictwoControllerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWydawnictwoModel(string id)
        {
            if (_context.WydawnictwoModel == null)
            {
                return NotFound();
            }
            var wydawnictwoModel = await _context.WydawnictwoModel.FindAsync(id);
            if (wydawnictwoModel == null)
            {
                return NotFound();
            }

            _context.WydawnictwoModel.Remove(wydawnictwoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WydawnictwoModelExists(string id)
        {
            return (_context.WydawnictwoModel?.Any(e => e.PublisherName == id)).GetValueOrDefault();
        }
    }
}
