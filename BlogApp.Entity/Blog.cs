﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Entity
{
   public class Blog
    {
        public int BlogId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Image { get; set; }

        [BindNever]
        public DateTime Date { get; set; }
        public bool isApproved { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        /// <summary>
        /// Ana sayfada olması isteniyorsa true değilse false
        /// </summary>
        public bool IsHome { get; set; }
        
        /// <summary>
        /// Slider'da görünecekse truedeğilse false
        /// </summary>
        public bool IsSlider { get; set; }
    }
}
