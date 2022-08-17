using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IBlogRepository blogRepository;
        public HomeController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        public IActionResult Index()
        {
            HomeBlogModel model = new HomeBlogModel();

            model.HomeBlogs = blogRepository.GetAll().Where(n => n.isApproved && n.IsHome).ToList();
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