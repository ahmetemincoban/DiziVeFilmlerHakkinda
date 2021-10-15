using Microsoft.AspNetCore.Identity;

namespace DiziveFilmHakkinda.Models
{
    public class AppRole : IdentityRole
    {
        public string Aciklama { get; set; }
    }
    
}