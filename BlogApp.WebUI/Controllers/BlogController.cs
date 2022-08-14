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
        public IActionResult AddOrUpdate(int? id)
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            if (!id.HasValue)
            {
                return View(new Blog());
            }
            else
            {
                return View(blogRepository.GetById(id.Value));
            }
        }

        [HttpPost]
        public IActionResult AddOrUpdate(Blog entity)
        {
            if (ModelState.IsValid)
            {
                blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.BlogId} nolu blog kaydedildi";
                return RedirectToAction(nameof(BlogController.List));
            }
            return View(entity);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(blogRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Delete(Blog blog)
        {
            blogRepository.DeleteBlog(blog.BlogId);
            return RedirectToAction(nameof(BlogController.List));
        }
    }
}