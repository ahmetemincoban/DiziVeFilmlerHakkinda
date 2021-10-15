using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DiziveFilmHakkinda.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using DiziveFilmHakkinda.Models;

namespace DiziveFilmHakkinda
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<IcerikDb>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("IcerikDbSqlite")));
           
            services.AddIdentity<AppUser,AppRole>(opt=>opt.SignIn.RequireConfirmedAccount=true)
            .AddEntityFrameworkStores<IcerikDb>().AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(opt=>
            {
                opt.Cookie.HttpOnly=true;
                //opt.ExpireTimeSpan=TimeSpan.FromMinutes(5);
                opt.LoginPath="/Identity/Account/Login";
                opt.AccessDeniedPath="/Identity/Account/AccessDenied";
                opt.SlidingExpiration=true;
            });
            services.AddSession(opt=>opt.IdleTimeout=TimeSpan.FromMinutes(5));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else 
            {
                app.UseStatusCodePagesWithRedirects("/IcerikKontrol/adresyok");
                app.UseExceptionHandler("/IcerikKontrol/adresyok");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=IcerikKontrol}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
            });
            
        }
    }
}
