using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiziveFilmHakkinda.Data;
using DiziveFilmHakkinda.Models;
using Microsoft.AspNetCore.Authorization;

namespace DiziveFilmHakkinda.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriApiController : ControllerBase
    {
        private readonly IcerikDb _context;

        public KategoriApiController(IcerikDb context)
        {
            _context = context;
        }

        // GET: api/KategoriApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KategoriModeli>>> GetKategoriler()
        {
            return await _context.Kategoriler.ToListAsync();
        }

        // GET: api/KategoriApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KategoriModeli>> GetKategoriModeli(int id)
        {
            var kategoriModeli = await _context.Kategoriler.FindAsync(id);

            if (kategoriModeli == null)
            {
                return NotFound();
            }

            return kategoriModeli;
        }

        // PUT: api/KategoriApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKategoriModeli(int id, KategoriModeli kategoriModeli)
        {
            if (id != kategoriModeli.Id)
            {
                return BadRequest();
            }

            _context.Entry(kategoriModeli).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategoriModeliExists(id))
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

        // POST: api/KategoriApi
        [HttpPost]
        public async Task<ActionResult<KategoriModeli>> PostKategoriModeli(KategoriModeli kategoriModeli)
        {
            _context.Kategoriler.Add(kategoriModeli);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKategoriModeli", new { id = kategoriModeli.Id }, kategoriModeli);
        }

        // DELETE: api/KategoriApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKategoriModeli(int id)
        {
            var kategoriModeli = await _context.Kategoriler.Include(x=>x.KategoriIcerikler).SingleOrDefaultAsync(x=>x.Id==id);
            if (kategoriModeli == null)
            {
                return NotFound();
            }
            _context.RemoveRange(kategoriModeli.KategoriIcerikler);
            _context.Kategoriler.Remove(kategoriModeli);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KategoriModeliExists(int id)
        {
            return _context.Kategoriler.Any(e => e.Id == id);
        }
    }
}
