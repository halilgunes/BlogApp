﻿using BlogApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogApp.Data.Abstract
{
    public interface IBlogRepository
    {
        Blog GetById(int blogId);
        IQueryable<Blog> GetAll();
        void AddBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(int blogId);
        void SaveBlog(Blog blog);
    }
}
