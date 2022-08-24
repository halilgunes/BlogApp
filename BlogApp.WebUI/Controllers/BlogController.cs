using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

            return View(query.OrderByDescending(x => x.Date).ToList());
        }

        public IActionResult List()
        {
            return View(blogRepository.GetAll());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View(new Blog());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Blog entity)
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
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(categoryRepository.GetAll(), "CategoryId", "Name");
            return View(blogRepository.GetById(id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Blog entity, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file.Length <= 0)
                {
                    return View(entity);
                }
                string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid().ToString().Substring(1, 10)}.{Path.GetExtension(file.FileName)}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                entity.Image = fileName;
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