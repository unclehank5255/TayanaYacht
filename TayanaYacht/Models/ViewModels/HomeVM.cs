using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TayanaYacht.Models.ViewModels
{
    public class HomeVM
    {
        public List<TayanaYacht.Models.News.News> NewsList { get; set; }
        public List<TayanaYacht.Models.Yacht.Yacht> Yachts { get; set; }

    }
}