using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WebUI.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepository repository;

        public CategoryController(ICategoryRepository repo)
        {
            repository = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(repository.GetAll());
        }

        [HttpGet]
        public IActionResult AddOrUpdate(int? id)
        {
            if (!id.HasValue)
            {
                return View(new Category());
            }
            else
            {
                return View(repository.GetById(id.Value));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrUpdate(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.SaveCategory(category);
                TempData["message"] = $"{category.Name} kategorisi güncellendi";
                return RedirectToAction(nameof(CategoryController.List));
            }
            return View(category);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            repository.DeleteCategory(id);
            return RedirectToAction(nameof(CategoryController.List));
        }

        [HttpGet]
        public IActionResult Delete(int CategoryId)
        {
            return View(repository.GetById(CategoryId));
        }
    }
}