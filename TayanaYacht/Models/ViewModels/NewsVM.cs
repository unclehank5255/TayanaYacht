using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TayanaYacht.Models.ViewModels
{
    public class NewsVM
    {
        public List<TayanaYacht.Models.News.News> NewsList { get; set; }
        public int TotalCount { get; set; }
        public int PublishedCount { get; set; }
        public DateTime? LastPostedAt { get; set; }

        public string NewsBannerPath { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}