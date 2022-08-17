using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Models
{
    public class HomeBlogModel
    {
        public List<Blog> HomeBlogs { get; set; }
        public List<Blog> SliderBlogs { get; set; }

    }
}
