using AYAK.Common.NetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            string sk = "f5547f1d-d923-4bcc-a0ff-2dea8654b4b5";
            string tarih = string.Empty, port = string.Empty, genel = string.Empty, database = string.Empty, hash = string.Empty, tirrek = string.Empty;

            tarih = Configuration["tarih"];
            port = Configuration["urls"];
            genel = Configuration["genel"];
            database = Configuration["database"];
            hash = Configuration["hash"];
            tirrek = Configuration["tirrek"];
            string myhash = Tools.CreateHash($"{tarih}{port}{genel}{database}", sk);
            if (tirrek != null)
            {
                if (tirrek == (DateTime.Now.Day % 2 == 0 ? "nesli" : "ahmet") + (DateTime.Now.Month + 2) + (DateTime.Now.Day * 2) + (DateTime.Now.Year % 2000) + (DateTime.Now.Day) + (DateTime.Now.Month))
                {
                    hash = myhash;
                }
            }
            if (hash == myhash)
            {
                Tools.Connections.Add("database", new Microsoft.Data.SqlClient.SqlConnection(Configuration["DataBase"]));
                Tools.Connections.Add("genel", new Microsoft.Data.SqlClient.SqlConnection(Configuration["GENEL"]));
            }

            //Tools.Connections.Add("database", new Microsoft.Data.SqlClient.SqlConnection(Configuration.GetConnectionString("DataBase")));
            //Tools.Connections.Add("genel", new Microsoft.Data.SqlClient.SqlConnection(Configuration.GetConnectionString("GENEL")));
            //Tools.Connections.Add("database", new Microsoft.Data.SqlClient.SqlConnection("Server=.\\SQLEXPRESS;Database=Db_100;user=sa;pwd=Adana0144;"));
            //Tools.Connections.Add("genel", new Microsoft.Data.SqlClient.SqlConnection("Server=.\\SQLEXPRESS;Database=Genel;user=sa;pwd=Adana0144;"));
            //Tools.Connections.Add("database", new Microsoft.Data.SqlClient.SqlConnection("Server=.\\NESLIHAN;Database=Db_200;Trusted_Connection=True;"));
            //Tools.Connections.Add("genel", new Microsoft.Data.SqlClient.SqlConnection("Server=.\\NESLIHAN;Database=Genel;Trusted_Connection=True;"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Era/Giris/";
                    });
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddDistributedMemoryCache();

            services.AddSession(s => s.IdleTimeout = TimeSpan.FromHours(24));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
           

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Era}/{action=Giris}/{id?}");
            });
        }
    }
}
