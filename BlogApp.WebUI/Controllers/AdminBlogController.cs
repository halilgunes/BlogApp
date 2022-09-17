using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.WebUI.Controllers
{
    [Authorize]
    public class AdminBlogController : Controller
    {
        IBlogRepository blogRepository;
        ICategoryRepository categoryRepository;
        private readonly UserManager<IdentityUser> userManager;
        public AdminBlogController(IBlogRepository _repo, ICategoryRepository _catRepo, UserManager<IdentityUser> userManager )
        {
            blogRepository = _repo;
            categoryRepository = _catRepo;
            this.userManager = userManager;
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
                return RedirectToAction(nameof(AdminBlogController.List));
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
                if (file != null && file.Length > 0)
                {
                    string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid().ToString().Substring(1, 10)}.{Path.GetExtension(file.FileName)}";
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    entity.Image = fileName;
                }
                blogRepository.SaveBlog(entity);
                TempData["message"] = $"{entity.BlogId} nolu blog kaydedildi";
                return RedirectToAction(nameof(AdminBlogController.List));
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
            return RedirectToAction(nameof(AdminBlogController.List));
        }

        [HttpGet]
        public IActionResult List()
        {
            return View(blogRepository.GetAll());
        }
    }
}