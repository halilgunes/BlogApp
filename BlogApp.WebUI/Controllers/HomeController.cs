using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
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

        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                return View(blogRepository.GetAll().Where(n => n.isApproved && n.IsHome && n.CategoryId == id.Value).ToList());
            }
            else
            {
                return View(blogRepository.GetAll().Where(n => n.isApproved && n.IsHome).ToList());
            }

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