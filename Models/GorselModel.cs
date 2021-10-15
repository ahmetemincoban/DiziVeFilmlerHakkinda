using System.ComponentModel.DataAnnotations;

namespace DiziveFilmHakkinda.Models
{
    public class GorselModel
    {
        public int ID { get; set; }
        public string DosyaAdi { get; set; }
        public int IcerigiId { get; set; }
        [Required]
        public IcerikModeli Icerigi { get; set; }
    }
}