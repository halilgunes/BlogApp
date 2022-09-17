using BlogApp.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {
        public static async void Seed(IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                BlogContext context = scope.ServiceProvider.GetRequiredService<BlogContext>();
                context.Database.Migrate();
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category() { Name = "Category 1" },
                        new Category() { Name = "Category 2" },
                        new Category() { Name = "Category 3" }
                    );

                    await context.SaveChangesAsync();
                }
                if (!context.Blogs.Any())
                {
                    context.Blogs.AddRange(

                        new Blog() { Title = "Blog title 1", Description = "Blog Description", Body = "Blog Body 1", Image = "1.jpeg", Date = DateTime.Now.AddDays(-5), isApproved = true, CategoryId = 1 },
                        new Blog() { Title = "Blog title 2", Description = "Blog Description", Body = "Blog Body 1", Image = "2.jpeg", Date = DateTime.Now.AddDays(-7), isApproved = true, CategoryId = 1 },
                        new Blog() { Title = "Blog title 3", Description = "Blog Description", Body = "Blog Body 1", Image = "3.jpeg", Date = DateTime.Now.AddDays(-8), isApproved = false, CategoryId = 2 },
                        new Blog() { Title = "Blog title 4", Description = "Blog Description", Body = "Blog Body 1", Image = "4.jpeg", Date = DateTime.Now.AddDays(-9), isApproved = true, CategoryId = 3 }
                    );

                    await context.SaveChangesAsync();
                }

                //Burada aslında servisler için yeni bir scope yaratıyor işi bitince kapatıyor.
                //Startup.cs içerisinde verilen servislere böylece erişebiliyoruz ancak başka bir scope yaratarak.

                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                appDbContext.Database.Migrate();

                if (!appDbContext.Users.Any())
                {
                    userManager.CreateAsync(new IdentityUser { Email = "deneme1@bla.com", UserName = "deneme1" }, "asd123").Wait();
                    userManager.CreateAsync(new IdentityUser { Email = "deneme2@bla.com", UserName = "deneme2" }, "asd123").Wait();
                }

            }
        }
    }
}
