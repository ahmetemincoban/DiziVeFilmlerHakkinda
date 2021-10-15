using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiziveFilmHakkinda.Data;
using DiziveFilmHakkinda.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DiziveFilmHakkinda.Controllers
{
    [Authorize]
    public class IcerikKontrol : Controller
    {
        #region //(Sign)
            //BU PROJE AHMET EMİN ÇOBAN VE AYBÜKE ARSLAN'A AİTTİR.
        #endregion
        private readonly IcerikDb _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private string _dosyaYolu;

        public IcerikKontrol(IcerikDb context, IWebHostEnvironment hostEnvironment,UserManager<AppUser> userManager)
        {
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _context = context;
            _dosyaYolu=Path.Combine(_hostEnvironment.WebRootPath,"Images");
            if(!Directory.Exists(_dosyaYolu))
                Directory.CreateDirectory(_dosyaYolu);
        }

        // GET: IcerikKontrol
        [AllowAnonymous]
        public async Task<IActionResult> Index(string aranan)
        {
            return View(await _context.Icerikler.Include(x=>x.Gorseller).Where(x=>x.Baslik.Contains(aranan) || aranan==null).ToListAsync());
        }
        public async Task<IActionResult> KategorininIcerikleri(int? id)
        {   
            if (id==null)
            {
                return RedirectToAction(nameof(adresyok));
            }
            var kat =await _context.Kategoriler.Include(x=>x.KategoriIcerikler).ThenInclude(x=>x.Icerik).ThenInclude(x=>x.Gorseller).SingleOrDefaultAsync(x=>x.Id==id);

                ViewBag.KategoriBilgi=$"{kat.Adi} kategorisine ait içerikler";
                ViewBag.kategoriId=kat.Id;
                return View("Index", kat.Icerikleri);
        }
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> KategoriDuzenle(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            var icerik = await _context.Icerikler.Include(x=>x.KategoriIcerikler).ThenInclude(x=>x.Kategori).SingleOrDefaultAsync(m => m.ID == id);
            if (icerik == null)
            {
                return RedirectToAction(nameof(adresyok));
            }
            return View(icerik);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriDuzenle(int? id, IFormCollection elemanlar)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            var icerik = await _context.Icerikler.Include(x=>x.KategoriIcerikler).SingleOrDefaultAsync(m => m.ID == id);
            if (icerik == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            var seciliKategoriler=elemanlar["SeciliKategoriler"];
            icerik.KategoriIcerikler.Clear();
            foreach (var item in seciliKategoriler)
            {
                icerik.KategoriIcerikler.Add(new KategorilerVeIcerikler{KategoriId=int.Parse(item)});
            }
            await _context.SaveChangesAsync();
            TempData["Mesaj"]=$"{icerik.Baslik} içeriğinin kategorileri başarıyla güncellendi";
            return RedirectToAction(nameof(Edit),new {id=icerik.ID});
        }
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> AdminSayfa()
        {
            return View(await _context.Icerikler.Include(x=>x.Gorseller).ToListAsync());
        }

        // GET: IcerikKontrol/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            var icerikModeli = await _context.Icerikler.Include(x=>x.Gorseller).Include(x=>x.KategoriIcerikler).ThenInclude(x=>x.Kategori)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (icerikModeli == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            return View(icerikModeli);
        }

        // GET: IcerikKontrol/Create
        [Authorize(Roles ="Admin,Moderator")]
        public IActionResult Create(int? id)
        {
            return View();
        }

        // POST: IcerikKontrol/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id,[Bind("Baslik,Tur,IMDB,Aciklama,GorselDosyalari,IcerikYayinlanmaDurumu")] IcerikModeli icerik, IFormCollection elemanlar)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in icerik.GorselDosyalari)
                {
                    var tamDosyaAdi=Path.Combine(_dosyaYolu,item.FileName);
                    using (var dosyaAkisi = new FileStream(tamDosyaAdi,FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi);
                    }
                    icerik.Gorseller.Add(new GorselModel{DosyaAdi=item.FileName});
                }
                if (id!=null) //Kategoriden id geldiğinde kategoriye içerik ekleme
                    icerik.KategoriIcerikler.Add(new KategorilerVeIcerikler{KategoriId=(int)id});
                _context.Add(icerik);
                 var seciliKategoriler=elemanlar["SeciliKategoriler"];
                foreach (var item in seciliKategoriler)
                {
                    icerik.KategoriIcerikler.Add(new KategorilerVeIcerikler{KategoriId=int.Parse(item)});
                }
                await _context.SaveChangesAsync();
                TempData["Mesaj"]=$"{icerik.Baslik} içeriği başarıyla eklendi.";
                if (id!=null) return RedirectToAction(nameof(KategorininIcerikleri),new {id=id});
                return RedirectToAction(nameof(Index));
            }
            return View(icerik);
        }

        // GET: IcerikKontrol/Edit/5
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            var icerik = await _context.Icerikler.Include(x=>x.Gorseller).Include(x=>x.KategoriIcerikler).ThenInclude(x=>x.Kategori).SingleOrDefaultAsync(x=>x.ID==id);
            if (icerik == null)
            {
                return RedirectToAction(nameof(adresyok));
            }
            return View(icerik);
        }

        // POST: IcerikKontrol/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Baslik,Tur,IMDB,Aciklama,IcerikYayinlanmaDurumu,IcerikOneCikartmaDurumu")] IcerikModeli icerik, IFormCollection elemanlar)
        {
            if (id != icerik.ID)
            {
                return RedirectToAction(nameof(adresyok));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(icerik);
                    var seciliKategoriler=elemanlar["SeciliKategoriler"];
                    icerik.KategoriIcerikler.Clear();
                    foreach (var item in seciliKategoriler)
                    {
                        icerik.KategoriIcerikler.Add(new KategorilerVeIcerikler{KategoriId=int.Parse(item)});
                    }
                    await _context.SaveChangesAsync();
                    TempData["mesaj"]=$"{icerik.Baslik} içeriği başarıyla güncellendi";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IcerikModeliExists(icerik.ID))
                    {
                        return RedirectToAction(nameof(adresyok));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(icerik);
        }

        // GET: IcerikKontrol/Delete/5
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            var icerikModeli = await _context.Icerikler.Include(x=>x.Gorseller).FirstOrDefaultAsync(m => m.ID == id);
            if (icerikModeli == null)
            {
                return RedirectToAction(nameof(adresyok));
            }

            return View(icerikModeli);
        }

        // POST: IcerikKontrol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var icerikModeli = await _context.Icerikler.FindAsync(id);
            _context.Icerikler.Remove(icerikModeli);

            try
            {
                await _context.SaveChangesAsync();
                foreach (var item in icerikModeli.Gorseller)
                {
                    System.IO.File.Delete(Path.Combine(_dosyaYolu,item.DosyaAdi));
                }
                TempData["mesaj"]=$"{icerikModeli.Baslik} içeriği başarıyla silindi";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                TempData["mesaj"]=$"{icerikModeli.Baslik} içeriği silinirken bir veritabanı hatası meydana geldi";
                return RedirectToAction(nameof(Index));
            }
            catch(IOException)
            {
                TempData["mesaj"]=$"{icerikModeli.Baslik} içeriğinin görselleri silinirken bir hata meydana geldi";
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> ResimSil(int id){
            var gorsel=await _context.Gorseller.FindAsync(id);
            _context.Gorseller.Remove(gorsel);
            await _context.SaveChangesAsync();
            // System.IO.File.Delete(Path.Combine(_dosyaYolu,gorsel.DosyaAdi));
            return RedirectToAction(nameof(Edit),new {id=gorsel.IcerigiId});
        }
        [HttpPost]
        [Authorize(Roles ="Admin,Moderator")]
        public async Task<IActionResult> ResimEkle(IcerikModeli icerik)
        {
            var resimEklenecekicerik = await _context.Icerikler.FindAsync(icerik.ID);
            try
            {
                foreach (var item in icerik.GorselDosyalari)
                {
                    var tamDosyaAdi = Path.Combine(_dosyaYolu, item.FileName);
                
                    using (var dosyaAkisi = new FileStream(tamDosyaAdi, FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi); //server'a upload
                    }

                    resimEklenecekicerik.Gorseller.Add( new GorselModel{DosyaAdi=item.FileName} ); 
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Edit),new {id=icerik.ID});
            }
            catch (System.Exception)
            {
                TempData["mesaj"]="Lütfen önce yüklenecek dosyaları seçiniz";
                return RedirectToAction(nameof(Edit),new {id=icerik.ID});
            }
        }

        

        private bool IcerikModeliExists(int id)
        {
            return _context.Icerikler.Any(e => e.ID == id);
        }

        public IActionResult adresyok(){
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
