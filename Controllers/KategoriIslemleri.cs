using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiziveFilmHakkinda.Data;
using DiziveFilmHakkinda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DiziveFilmHakkinda.Controllers
{
    [Authorize(Roles ="Admin,Moderator")]
    public class KategoriIslemleri : Controller
    {
        private readonly IcerikDb _context;
        private readonly UserManager<AppUser> _usermanager;

        public KategoriIslemleri(IcerikDb context,UserManager<AppUser> userManager)
        {
            _context = context;
            _usermanager=userManager;
        }

        // GET: KategoriIslemleri
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategoriler.ToListAsync());
        }

        // GET: KategoriIslemleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }

            var kategoriModeli = await _context.Kategoriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategoriModeli == null)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }

            return View(kategoriModeli);
        }

        // GET: KategoriIslemleri/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KategoriIslemleri/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adi")] KategoriModeli kategoriModeli)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategoriModeli);
                await _context.SaveChangesAsync();
                TempData["mesaj"]="Kategori Ekleme İşlemi Başarıyla Gerçekleştirildi.";
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriModeli);
        }

        // GET: KategoriIslemleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }

            var kategoriModeli = await _context.Kategoriler.FindAsync(id);
            if (kategoriModeli == null)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }
            return View(kategoriModeli);
        }

        // POST: KategoriIslemleri/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adi")] KategoriModeli kategoriModeli)
        {
            if (id != kategoriModeli.Id)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategoriModeli);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriModeliExists(kategoriModeli.Id))
                    {
                        return RedirectToAction("adresyok","IcerikKontrol");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriModeli);
        }

        // GET: KategoriIslemleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }

            var kategoriModeli = await _context.Kategoriler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategoriModeli == null)
            {
                return RedirectToAction("adresyok","IcerikKontrol");
            }

            return View(kategoriModeli);
        }

        // POST: KategoriIslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var kategoriModeli = await _context.Kategoriler.FindAsync(id);
                _context.Kategoriler.Remove(kategoriModeli);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["mesaj"]="İçerisinde kayıtlar bulunan bir kategoriyi silemezsiniz!";
            }
            return RedirectToAction("KategorininIcerikleri","IcerikKontrol", new {id=id});
        }

        private bool KategoriModeliExists(int id)
        {
            return _context.Kategoriler.Any(e => e.Id == id);
        }
        public IActionResult KategoriYonetimiSpa()
        {
          return View();
        }
    }
}
