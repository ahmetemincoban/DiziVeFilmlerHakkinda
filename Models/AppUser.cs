using Microsoft.AspNetCore.Identity;

namespace DiziveFilmHakkinda.Models
{
    public class AppUser : IdentityUser 
    {
        public string Aciklama { get; set; }
    }
}