using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiziveFilmHakkinda.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiziveFilmHakkinda
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = CreateHostBuilder(args).Build();
           Console.WriteLine("Sunucu başlatılıyor lütfen bekleyiniz...");
           using (var scope = host.Services.CreateScope())
           {
               var servis = scope.ServiceProvider;
               try
               {
                   SeedData.Initialize(servis);
               }
               catch (Exception hata)
               {
                   var logkaydi = servis.GetRequiredService<ILogger<Program>>();
                   logkaydi.LogError(hata,"Çekirdek veriler veritabanına eklenirken bir hata oluştu.");
               }
           }
           host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
