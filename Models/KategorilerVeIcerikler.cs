using System;

namespace DiziveFilmHakkinda.Models
{
    public class KategorilerVeIcerikler
    {
        //Reference
        public IcerikModeli Icerik { get; set; }
        public KategoriModeli Kategori { get; set; }
        //Scaler
        public int IcerikId { get; set; }
        public int KategoriId { get; set; }
        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }
}