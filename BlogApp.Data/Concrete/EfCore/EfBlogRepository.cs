using BlogApp.Data.Abstract;
using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return context.Blogs.FirstOrDefault(b => b.BlogId == blogId);
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
                entity.Image = blog.Image;
                context.Blogs.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
