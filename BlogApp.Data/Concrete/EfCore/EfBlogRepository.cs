using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace BlogApp.Data.Concrete.EfCore
{
    public class EfBlogRepository : IBlogRepository
    {
        private BlogContext context;
        public EfBlogRepository(BlogContext context)
        {
            this.context = context;
        }

        public void AddBlog(Blog blog)
        {
            context.Blogs.Add(blog);
            context.SaveChanges();
        }

        public void SaveBlog(Blog blog)
        {
            if (blog != null)
            {
                if (blog.BlogId == 0)
                {
                    AddBlog(blog);
                }
                else
                {
                    UpdateBlog(blog);
                }
            }
        }

        public async void DeleteBlog(int blogId)
        {
            Blog blog = context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
            if (blog != null)
            {
                context.Blogs.Remove(blog);
                await context.SaveChangesAsync();
            }
        }

        public IQueryable<Blog> GetAll()
        {
            return context.Blogs;
        }

        public Blog GetById(int blogId)
        {
            Blog ret = context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
           // ret.Body = HttpUtility.HtmlEncode(ret.Body);
            return ret;
        }

        public void UpdateBlog(Blog blog)
        {
            //context.Entry(blog).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Blog entity = GetById(blog.BlogId);
            if (blog != null)
            {
                entity.CategoryId = blog.CategoryId;
                entity.Title = blog.Title;
                entity.Description = blog.Description;
                if (!string.IsNullOrWhiteSpace(blog.Image))
                {
                    entity.Image = blog.Image; 
                }
                entity.IsHome = blog.IsHome;
                entity.isApproved = blog.isApproved;
                entity.Date = DateTime.Now;
                entity.IsSlider = blog.IsSlider;
                entity.Body = blog.Body;
                context.Blogs.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
