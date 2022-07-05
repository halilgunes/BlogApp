using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogApp.WebUI.Controllers
{
    public class BlogController : Controller
    {
        IBlogRepository blogRepository;
        ICategoryRepository categoryRepository;
        public BlogController(IBlogRepository _repo, ICategoryRepository _catRepo)
        {
            blogRepository = _repo;
            categoryRepository = _catRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(blogRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(),"CategoryId","Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            entity.Date = DateTime.Now;
                 
            if (ModelState.IsValid)
            {
                blogRepository.AddBlog(entity);
                return RedirectToAction(nameof(List));
            }
            return View(entity);
        }

    }
}