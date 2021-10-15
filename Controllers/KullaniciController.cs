using System.Linq;
using System.Threading.Tasks;
using DiziveFilmHakkinda.Data;
using DiziveFilmHakkinda.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiziveFilmHakkinda.Controllers
{
    [Authorize(Roles ="Admin")]
    public class KullaniciController : Controller
    {
        private readonly DbContext context;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;

        public KullaniciController(IcerikDb context,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string aranan)
        {
            var kullanicilar= await userManager.Users.Where(x=>x.Email.Contains(aranan)||aranan==null).ToListAsync();
            return View(kullanicilar); 
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel kullanici)
        {   
            var yeniKullanici=new AppUser { UserName = kullanici.Eposta, Email = kullanici.Eposta, EmailConfirmed = true };
            await userManager.CreateAsync(yeniKullanici, kullanici.Sifre);
            await userManager.AddToRoleAsync(yeniKullanici,"User");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            await userManager.DeleteAsync(await userManager.FindByIdAsync(id));
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RolAyarla(string id)
        {
            var kullanici = await userManager.FindByIdAsync(id);
            var kullaniciRol = await userManager.GetRolesAsync(kullanici);
            ViewBag.KullaniciAdi = kullanici.UserName;
            return View(kullaniciRol);
        }
        [HttpPost]
        public async Task<IActionResult> RolAyarla(string id, IFormCollection elemanlar)
        {
            var kullanici = await userManager.FindByIdAsync(id);
            var rolleri = elemanlar["Rolleri"];
            await userManager.RemoveFromRolesAsync(kullanici, await userManager.GetRolesAsync(kullanici));
            await userManager.AddToRolesAsync(kullanici, rolleri.ToList());
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}