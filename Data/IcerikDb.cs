using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiziveFilmHakkinda.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DiziveFilmHakkinda.Data
{
    public class IcerikDb : IdentityDbContext<AppUser,AppRole,string>
    {
        public IcerikDb (DbContextOptions<IcerikDb> options)
            : base(options)
        {
            
        }

        public DbSet<IcerikModeli> Icerikler { get; set; }
        public DbSet<GorselModel> Gorseller { get; set; }
        public DbSet<KategoriModeli> Kategoriler { get; set; }
        public DbSet<KategorilerVeIcerikler> KategorilerVeIceriklerListe { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IcerikModeli>()
                .HasMany(x=>x.Kategorileri)
                .WithMany(x=>x.Icerikleri)
                .UsingEntity<KategorilerVeIcerikler>(
                    j=>j.HasOne(x=>x.Kategori).WithMany(x=>x.KategoriIcerikler).HasForeignKey(x=>x.KategoriId).OnDelete(DeleteBehavior.Restrict),
                    j=>j.HasOne(x=>x.Icerik).WithMany(x=>x.KategoriIcerikler).HasForeignKey(x=>x.IcerikId).OnDelete(DeleteBehavior.Cascade),
                    j=>{
                        j.HasKey(x=>new {x.IcerikId,x.KategoriId});
                    });
        }
    }
}