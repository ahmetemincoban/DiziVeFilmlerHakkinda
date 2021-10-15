using DiziveFilmHakkinda.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiziveFilmHakkinda.Data
{
//     
public class SeedData
    {
        public async static void Initialize(IServiceProvider serviceProvider){
            Console.WriteLine("Veritabanı işlemleri için gerekli kontroller sağlanıyor...");
            using (var context = new IcerikDb(serviceProvider.GetRequiredService<DbContextOptions<IcerikDb>>()))
            {
                context.Database.Migrate();
                if (context.Icerikler.Any())
                {
                    return;
                }
                Console.WriteLine("İçerik bulunamadı çekirdek veriler ekleniyor...");
#region //Kullanıcı ve Rol Yönetimi
                var userManager=serviceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager=serviceProvider.GetRequiredService<RoleManager<AppRole>>();
                var admin=new AppUser{Email="admin@gmail.com",UserName="admin@gmail.com",EmailConfirmed=true};
                var moderator=new AppUser{Email="moderator@gmail.com",UserName="moderator@gmail.com",EmailConfirmed=true};
                var user=new AppUser { Email = "user@gmail.com", UserName = "user@gmail.com", EmailConfirmed = true };
                await roleManager.CreateAsync(new AppRole{Name="Admin",Aciklama="En Yüksek Yetkidir!"});
                await roleManager.CreateAsync(new AppRole{Name="Moderator",Aciklama="En Yüksek 2. Yetkidir!"});
                await roleManager.CreateAsync(new AppRole{Name="User",Aciklama="Standart Kullanıcılar İçin Olan Yetkidir!"});
                await userManager.CreateAsync(admin,"Admin123*");
                await userManager.CreateAsync(moderator,"Moderator123*");
                await userManager.CreateAsync(user,"User123*");
                await userManager.AddToRolesAsync(admin,new List<string>{"Admin","Moderator","User"});
                await userManager.AddToRolesAsync(moderator,new List<string>{"Moderator","User"});
                await userManager.AddToRoleAsync(user,"User");
#endregion
#region //İçeriklerin Eklenmesi
                 var iceriklerim= new IcerikModeli[]
                {
                    new IcerikModeli{
                        Baslik="Doctor Who?",Tur="Dizi",IMDB=8.6,Aciklama="Doctor Who dizisi her ne kadar polisiye dizisi olarak görülse de aslında bilim kurgu ağırlıklı bir dizidir. Dizinin başkahramanı olan Doctor Who kendisini bir zaman yolcusu olarak tarif etmektedir. Doctor Who uzayda ve zamanda yolculuk yaparak tüm canlılara yardım götüren bi baş kahramandır. TARDIS adı verilen polis kulübesi ile yolculuk yapar. TARDIS İngilizler tarafından kurulan 1929 yılında tasarlanmış acil durumlarda kullanılabilecek police box olarak da bilinen bir polis kulübesidir. Doctor Who yaptığı tüm yolculukları bu kulübe ile yapmaktadır. Bu dizide kahramanın adının Doctor Who olmasının sebebi ilk sezonda açıklanmamış olan doktorun adının evrenin güzelliğini sağlayan kilit açıcı özellikte veya gizli bir şifre ile karakterler olduğunun belirtilmesidir. Bu sebeple adı açıklanmamış ve Doctor Who olarak bilinmektedir. Tüm sezon boyuncada ismi açıklanmamış ve böyle devam etmiştir.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel(){DosyaAdi="dw.jpg"},
                            new GorselModel{DosyaAdi="dw1.png"},
                            new GorselModel{DosyaAdi="dw2.jpg"},
                        },IcerikYayinlanmaDurumu=true,IcerikOneCikartmaDurumu=true

                    },
                    new IcerikModeli{
                        Baslik="Black Mirror",Tur="Dizi",IMDB=8.8,Aciklama="3 bölümden oluşan mini dizinin her bir bölümü bir başka hikayeyi anlatıyor. İlk bölüm, sosyal medyanın başa bela olabileceğini gösterirken, ikinci bölüm televizyondaki yarışmaların birgün varabileceği noktaya parmak basıyor. Üçüncü bölüm ise; yakın bir gelecekte insanlar taktırdıkları hafıza çipleri sayesinde tüm hayatlarını depolayabilmektedirler. Hiçbir şey unutulmuyor, hiçbir ayrıntı atlanmıyordur. Peki ya bu, iyi bir şey midir?",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="blackmirror.jpg"},
                            new GorselModel{DosyaAdi="bm.jpg"},
                        },IcerikYayinlanmaDurumu=true,IcerikOneCikartmaDurumu=false
                    },
                    new IcerikModeli{
                        Baslik="The Queen’s Gambit",Tur="Dizi",IMDB=8.6,Aciklama="Dünyanın en iyi satranç oyuncularından biri olan Beth Harmon’ın hayatına odaklanılan dizinin başrolünü Anya Taylor-Joy üstleniyor. Ailesini kaybetmesinin ardından kurum tarafından büyütülen Beth, burada çalışan bir görevliden satranç oynamayı öğrenir. Satranç'ta olağanüstü bir yetenek sergileyen Beth, önüne çıkan tüm rakipleri yenmeyi başarır.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="gambit.jpg"},
                            new GorselModel{DosyaAdi="g1.jpg"},
                        },IcerikYayinlanmaDurumu=true,IcerikOneCikartmaDurumu=true
                    },
                    new IcerikModeli{
                        Baslik="Harry Potter 5",Tur="Film",IMDB=7.6,Aciklama="Harry Potter'ın ailesi ağır ve şaibeli bir trafik kazasında ölmüştür. Öksüz ve yetim kalan Harry'nin sığınabileceği tek yer, arasının pek de iyi olmadığı teyzesinin yanıdır. Harry, tüm hayatı boyunca idari ailesi tarafından kötü davranışlarla büyütülür. Ancak Harry Potter artık 11 yaşındadır ve Harry'nin hayalleri ve yetenekleri günden güne su yüzüne çıkmaktadır. Kısa süre sonra Hogwarts büyücülük okuluna davet edilir. Artık tek amacı, ailesinin bu şüpheli kazasını araştırmak ve muhattaplarını cezalandırmaktır.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="harrypotter.jpg"},
                            new GorselModel{DosyaAdi="hp1.jpg"},
                            new GorselModel{DosyaAdi="hp2.jpg"}
                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=true
                    },
                     new IcerikModeli{
                        Baslik="Hobbit",Tur="Film",IMDB=7.8,Aciklama="Bilbo huzurlu Hobbit toprakları olan The Shire'da yaşarken bir gün büyücü Gandalf aniden ortaya çıkar ve baş kahramanımız Bilbo kendisini efsanevi savaşçı Thorin tarafından yönetilen 13 cücelik maceracı bir grupta buluverir. Ejder Smaug’dan Erebor’un kayıp Cüce Krallığı’nı geri almak için çıktıkları bu yolculukta çirkin Goblinler, Orklar, öldürücü Warglar, Dev Örümcekler ve Büyücülerle dolu yollardan geçeceklerdir.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="hobbit1.jpg"},
                            new GorselModel{DosyaAdi="hobbit3.jpg"},
                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=false
                    },
                     new IcerikModeli{
                        Baslik="Interstellar",Tur="Film",IMDB=8.6,Aciklama="Teknik bilgisi ve becerisi yüksek olan Cooper, geniş mısır tarlalarında çiftçilik yaparak geçinmektedir; amacı iki çocuğuna güvenli bir hayat sunmaktır. Onlarla yaşayan Büyükbaba Donald çocuklara göz kulak olurken, henüz 10 yaşındaki kızı Murph şaşırtıcı bir zekaya sahiptir. Geçmişte bıraktığı biliminsanı kariyerini özleyen Cooper'un karşısına bir gün beklenmedik bir teklif çıkar ve ailesinin, dahası insanlığın güvenliği için zorlu bir karar alması gerekir.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="interstellar.jpg"},
                            new GorselModel{DosyaAdi="interstellar2.jpg"},
                            new GorselModel{DosyaAdi="interstellar3.jpg"}
                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=false
                    },
                     new IcerikModeli{
                        Baslik="Lucy",Tur="Film",IMDB=6.4,Aciklama="Tayvan'ın başkenti Taipei'nin suça batmış yeraltı dünyası sokak çeteleri, mafya ve işbirlikçi polisler tarafından yönetilirken en aktif ticaret, uyuşturucu ağı üzerinden yürütülür. Eğlenmeyi seven, sıradan bir genç kadın olan Lucy, birkaç gece beraber takıldığı Richard yüzünden kendisini bir anda en azılı uyuşturucu şebekelerinin birinin içine düşmüş bulur. Vücudunun içine kurye olması için yerleştirilen yeni bir tür sentetik uyuşturucu, beklenmedik bir şekilde Lucy'nin vücuduna nüfuz edip kanına karışmaya başlayınca mucizevi bir durumla yüzleşir. Lucy'in damarlarında dolaşan kimyasallar, ona insanüstü yetenekler kazandırmıştır! Artık akıl okuma, telekinezi ve acıyı hissetmeme gibi güçlere sahip olan genç kadın beyinin tüm algı kapılarını sonuna kadar açacaktır.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="lucy.jpg"},
                            new GorselModel{DosyaAdi="lucy1.jpg"},

                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=true
                    },
                     new IcerikModeli{
                        Baslik="Narcos",Tur="Dizi",IMDB=8.8,Aciklama="Uyuşturucu İle Mücadele Dairesi’nde çalışan bir Meksikalı bir ajan olan Javier Pena, ünlü Kolombiyalı uyuşturucu kaçakçısı Pablo Escobar’ı yakalamak için Birleşik Devletler tarafından Kolombiya’ya yollanır.Escobar'ın gerçek hayat hikayesinden uyarlanan dizi, dünya çapına yayılmış bir uyuşturucu şebekesini engellemeye çalışan kolluk kuvvetlerini konu ediniyor.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="n1.webp"},
                            new GorselModel{DosyaAdi="n4.jpg"}
                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=false
                    },
                     new IcerikModeli{
                        Baslik="Peaky Blinders",Tur="Dizi",IMDB=8.8,Aciklama="Birinci Dünya Savaşı’ndan yeni çıkmış İngiltere’yi, yasadışı faaliyet gösteren çeşitli çeteler sarmıştır. Bunlardan biri de soygunculuk ve at yarışıyla para kazanan Peaky Blinders’dır. Polislere rüşvet yedirerek paçayı kurtaran çete, bir soygunda yanlışlık yapınca, başlarına bela olacak yeni bir müfettişin şehre gelmesine vesile olurlar.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="peaky.jpg"},
                            new GorselModel{DosyaAdi="pb1.jpg"},
                        },IcerikYayinlanmaDurumu=true,IcerikOneCikartmaDurumu=true
                    },
                     new IcerikModeli{
                        Baslik="Zindan Adası",Tur="Film",IMDB=8.2,Aciklama="Teddy Daniels ve Chuck Aule isimli iki polis memurunun, Rachel Solando adlı bir akıl hastasının ortadan kaybolması üzerine tehlikeli akıl hastalarının tedavi gördüğü Shutter Adası isimli bölgede konuşlanan Ashecliffe Hastanesi'ne soruşturma yapmak için gitmesi ve sonradan gelişen esrarengiz olaylar aktarılıyor. Burada karşılaştıkları isyan tablosu ve çığrından çıkan işler bu davayı gittikçe zora sokacak, zamanla rüya ve gerçek arasındaki sınırlar zorlanacaktır.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="zindanadası.jpg"},
                            new GorselModel{DosyaAdi="si2.jpg"},
                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=false
                    },
                      new IcerikModeli{
                        Baslik="Siyah Kuğu",Tur="Film",IMDB=8.0,Aciklama="Genç bir kadın olan Nina, yetenekli bir balerindir. Eski bir balerin olan ve dans konusundaki hırsını kendisine aşılayan annesi ile New York’ta yaşayan Nina’nın hayatı danstan ibarettir. Bale yönetmeni Thomas Leroy, sahneye koyduğu Kuğu Gölü Balesi’nin baş dansçısını yeni sezonda değiştirmeye karar verir. Zarif, masum ve saf Beyaz Kuğu ile kötülüğün, şehvetin ve bilinmezliğin temsilcisi Siyah Kuğu'yu aynı anda canlandırabilecek bir balerin arayan yönetmenin ilk tercihi Nina olur. Ancak rolü almak için elinden geleni yapan Nina’nın karşısında güçlü bir rakibi vardır. Nina, Beyaz Kuğu rolü için harikalar yaratsa da genç kadının Siyah Kuğu performansı pek de başarılı değildir. Rakibi Lily ise Siyah Kuğu rolü için iyi bir performans sergiler. Lily ve Nina arasındaki rekabet, çalışmalar boyunca ilginç bir dostluğa dönüşür. Bu süreçte Nina, hayatının mahvolmasına neden olan karanlık tarafıyla yüzleşmeye başlar.",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="siyahkugu.jpg"},
                            new GorselModel{DosyaAdi="sk4.jpg"}
                        },IcerikYayinlanmaDurumu=true,IcerikOneCikartmaDurumu=true
                    },
                      new IcerikModeli{
                        Baslik="The Conjurıng",Tur="Film",IMDB=7.5,Aciklama="doğaüstü olayları inceleyip aydınlatmaya çalışan dünyaca ünlü çift Ed ve Lorraine Warren'ın karşılaştıkları ürkütücü bir vakayı ele alır. Ed ve Lorraine Warren bir gün Perron ailesinden bir telefon aldıklarında hayatlarının en korkutucu görevine atıldıklarının farkında değildir. Perron ailesinin gözlerden uzak çiftlik evi nedeni bilinmeyen karanlık bir varlık tarafından kuşatılmıştır ve bu nedenle de hayatları tam bir kabusa dönüşmüştür. Bu vakayı çözebileceklerine inanan deneyimli Warren çifti, ne kadar şeytani bir varlıkla karşı karşıya olduklarını çok geç fark edeceklerdir...",EklenmeTarihi=DateTime.Now,
                        Gorseller= new List<GorselModel>{
                            new GorselModel{DosyaAdi="conjuring.jpg"},
                        },IcerikYayinlanmaDurumu=false,IcerikOneCikartmaDurumu=false
                    },
                };
#endregion
#region //Kategorilerin Eklenmesi

                var kategorilerim=new KategoriModeli[]{
                    new KategoriModeli{Adi="Bilim Kurgu"},
                    new KategoriModeli{Adi="Dram"},
                    new KategoriModeli{Adi="Macera"},
                    new KategoriModeli{Adi="Aksiyon"},
                    new KategoriModeli{Adi="Komedi"},
                    new KategoriModeli{Adi="Animasyon"},
                    new KategoriModeli{Adi="Korku"},
                    new KategoriModeli{Adi="Gerilim"},
                    new KategoriModeli{Adi="Yerli"},
                    new KategoriModeli{Adi="Yabancı"},
                    new KategoriModeli{Adi="Polisiye"},
                    new KategoriModeli{Adi="Fantastik"},
                    new KategoriModeli{Adi="Aile"},
                    new KategoriModeli{Adi="Biyografi"},
                    new KategoriModeli{Adi="Gençlik"},
                    new KategoriModeli{Adi="Psikolojik"},
                    new KategoriModeli{Adi="Gizem"},
                    new KategoriModeli{Adi="Dizi"},
                    new KategoriModeli{Adi="Film"}
                };
#endregion
#region //Kategoriler ve İçeriklerin Birleştirilmesi
                context.KategorilerVeIceriklerListe.AddRange(
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[0]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[1]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[2]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[3]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[9]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[11]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[12]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[0], Kategori=kategorilerim[17]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[1], Kategori=kategorilerim[17]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[2], Kategori=kategorilerim[17]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[3], Kategori=kategorilerim[18]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[4], Kategori=kategorilerim[18]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[5], Kategori=kategorilerim[18]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[6], Kategori=kategorilerim[18]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[7], Kategori=kategorilerim[17]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[8], Kategori=kategorilerim[17]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[9], Kategori=kategorilerim[18]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[10], Kategori=kategorilerim[18]},
                    new KategorilerVeIcerikler{Icerik=iceriklerim[11], Kategori=kategorilerim[18]}
                );
#endregion                
                

                context.Icerikler.AddRange(iceriklerim);
                context.Kategoriler.AddRange(kategorilerim);
                await context.SaveChangesAsync();
                var logger=serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("Çekirdek veriler başarıyla eklendi.");
            }
        }
    }
}