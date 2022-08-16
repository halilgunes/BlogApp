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

        public IActionResult Index(int? id)
        {
            if (!id.HasValue)
            {
                return View(blogRepository.GetAll().Where(n => n.isApproved).OrderByDescending(x => x.Date).ToList());
            }
            else
            {
                return View(blogRepository.GetAll().Where(n => n.isApproved && n.CategoryId == id.Value).OrderByDescending(x => x.Date).ToList());
            }
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Blog blog)
        {
            blogRepository.DeleteBlog(blog.BlogId);
            return RedirectToAction(nameof(BlogController.List));
        }

        public IActionResult Details(int id)
        {
            return View(blogRepository.GetById(id));
        }
    }
}