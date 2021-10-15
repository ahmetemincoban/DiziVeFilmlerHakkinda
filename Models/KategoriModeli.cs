using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiziveFilmHakkinda.Models
{
    public class KategoriModeli
    {
        public int Id { get; set; }
        [Display (Name = "Adı")]
        public string Adi { get; set; }
        public List<IcerikModeli> Icerikleri { get; set; }=new List<IcerikModeli>();
        public List<KategorilerVeIcerikler> KategoriIcerikler { get; set; } =new List<KategorilerVeIcerikler>();

    }
}