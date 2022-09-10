using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Details(int id)
        {
            return View(blogRepository.GetById(id));
        }
    }
}