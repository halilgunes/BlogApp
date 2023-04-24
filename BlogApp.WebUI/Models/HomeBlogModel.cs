using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.WebUI.Models
{
    public class HomeBlogModel
    {
        public List<BlogGroup> HomeBlogs { get; set; }
        public List<Blog> SliderBlogs { get; set; }

    }
}
