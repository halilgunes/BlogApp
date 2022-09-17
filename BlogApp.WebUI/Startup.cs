using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;
using BlogApp.Data.Abstract;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.WebUI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("BlogApp.WebUI")));
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("BlogApp.WebUI")));
            services.AddTransient<IBlogRepository, EfBlogRepository>();
            services.AddTransient<ICategoryRepository, EfCategoryRepository>();
            services.AddMvc(options => options.EnableEndpointRouting = false); ;
            
            //Identiy bilgileri dbde tutulacak,kullanıcının email adresi unique olacak
            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
            {
                opt.Password.RequireUppercase = false;

                opt.Password.RequireNonAlphanumeric = false;

                opt.Password.RequiredLength = 4;

                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();//authentice olmalı kullanıcı diyoruz bu satırla.
            app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
       

            SeedData.Seed(app);

           
        }
    }
}
