using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        IBlogRepository blogRepository;
        ICategoryRepository categoryRepository;
        IConfiguration configuration;
        short rowCount = 3;
        
        public BlogController(IBlogRepository _repo, ICategoryRepository _catRepo,IConfiguration configuration)
        {
            blogRepository = _repo;
            categoryRepository = _catRepo;
            this.configuration = configuration;
            string val = configuration.GetValue<string>("BlogRowCount");
            if (!string.IsNullOrWhiteSpace(val))
            {
                rowCount = Convert.ToInt16(val);
            }
        }

        public IActionResult Index(int? id, string q)
        {
            var query = blogRepository.GetAll().Where(n => n.isApproved);
            if (id.HasValue)
            {
                query = query.Where(n => n.CategoryId == id.Value);
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(n => EF.Functions.Like(n.Title, "%" + q + "%") || EF.Functions.Like(n.Description, "%" + q + "%") || EF.Functions.Like(n.Body, "%" + q + "%"));
            }
            List<BlogGroup> groups = GetBlogGroup(query.OrderByDescending(x => x.Date).ToList());
            return View(groups);
        }

        public IActionResult List()
        {
            List<Blog> blogs = blogRepository.GetAll().ToList();
            List<BlogGroup> groups = GetBlogGroup(blogs);
            return View(groups);

        }

        private List<BlogGroup> GetBlogGroup(List<Blog> blogs)
        {
            List<BlogGroup> groups = new List<BlogGroup>();
            for (int i = 0; i < blogs.Count; i++)
            {
                if (i % rowCount == 0)
                {
                    groups.Add(new BlogGroup());
                }
                groups.LastOrDefault()?.BlogList.Add(blogs[i]);
            }

            return groups;
        }

        public IActionResult Details(int id)
        {
            return View(blogRepository.GetById(id));
        }
    }
}