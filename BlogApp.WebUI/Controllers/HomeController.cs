using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IBlogRepository blogRepository;
        IConfiguration configuration;
        public HomeController(IBlogRepository blogRepository,IConfiguration configuration)
        {
            this.blogRepository = blogRepository;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            HomeBlogModel model = new HomeBlogModel();
            short rowCount = 3;
            string val = configuration.GetValue<string>("BlogRowCount");
            if (!string.IsNullOrWhiteSpace(val))
            {
                rowCount = Convert.ToInt16(val);
            }
            List<Blog> blogs = blogRepository.GetAll().Where(n => n.isApproved && n.IsHome).ToList();
            List<BlogGroup> groups = new List<BlogGroup>();
            for (int i = 0; i < blogs.Count; i++)
            {
                if (i % rowCount == 0)
                {
                    groups.Add(new BlogGroup());
                }
                groups.LastOrDefault()?.BlogList.Add(blogs[i]);
            }

            model.HomeBlogs = groups;
            model.SliderBlogs = blogRepository.GetAll().Where(n => n.isApproved && n.IsSlider).ToList();

            return View(model);
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}