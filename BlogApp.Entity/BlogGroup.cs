using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Entity
{
    public class BlogGroup
    {
        public BlogGroup()
        {
            BlogList = new List<Blog>();
        }
        public List<Blog> BlogList { get; set; }
    }
}
