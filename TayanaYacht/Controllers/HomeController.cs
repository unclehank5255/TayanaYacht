using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.ViewModels;
using TayanaYacht.Models.Yacht;
namespace TayanaYacht.Controllers
{
    public class HomeController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();
        [AllowAnonymous]
        public ActionResult Index()
        {
            var vm = new HomeVM()
            {
                Yachts = db.Yachts.Where(y => y.DeletedAt == null && y.IsPublished == true).OrderByDescending(y => y.DisplayOrder).ThenByDescending(y => y.Id).Take(6).ToList(),
                NewsList = db.News.Where(n => n.DeletedAt == null && n.UnpublishedAt == null).OrderByDescending(n => n.PublishDate).ThenByDescending(n => n.Id).Take(3).ToList()
            };
            return View(vm);
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}