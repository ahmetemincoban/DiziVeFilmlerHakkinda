using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DiziveFilmHakkinda.Models
{
    public class IcerikModeli
    {
        public IcerikModeli(){
            Gorseller=new List<GorselModel>();
        }
        [Key]
        public int ID { get; set; }
        [StringLength (50,ErrorMessage="{0} en fazla 50 karakter olabilir")]
        [Required (ErrorMessage="{0} alanı boş bırakılamaz")]
        [Display (Name="Başlık")]        public string Baslik { get; set; }
        [Required (ErrorMessage="{0} alanı boş bırakılamaz")]
        [Display (Name="Tür")]
        public string Tur { get; set; }
        [DisplayFormat (ApplyFormatInEditMode=false)]
        [Required (ErrorMessage="{0} alanı boş bırakılamaz")]
        [Range(0, 10, ErrorMessage = "0 ile 10 arasında bir değer girebilirsiniz")]  
        [Display (Name="IMDB Puanı")]
        public double IMDB { get; set; }
        [Required (ErrorMessage="{0} alanı boş bırakılamaz")]
        [Display (Name="Açıklama")]
        public string Aciklama { get; set; }

        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

        [NotMapped]
        [Display (Name="İçeriğin Görselleri")]
        public IFormFile[] GorselDosyalari { get; set; }
        public List<GorselModel> Gorseller { get; set; }
        [Required (ErrorMessage="{0} alanı boş bırakılamaz")]
        [Display (Name="Yayınlanma Durumu")]
        public bool IcerikYayinlanmaDurumu { get; set; }        
        public bool IcerikOneCikartmaDurumu { get; set; }
        public List<KategoriModeli> Kategorileri { get; set; } = new List<KategoriModeli>();
        public List<KategorilerVeIcerikler> KategoriIcerikler { get; set; } =new List<KategorilerVeIcerikler>();

    }
}