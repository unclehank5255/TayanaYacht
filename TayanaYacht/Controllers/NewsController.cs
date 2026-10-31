using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.News;

namespace TayanaYacht.Controllers
{
    public class NewsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: News
        public ActionResult Index()
        {
            ViewBag.NewsBannerPath = db.NewsPages.Find(1)?.NewsBannerPath;
            return View(db.News.Where(n => n.DeletedAt == null && n.UnpublishedAt == null).OrderByDescending(n => n.CreatedAt).ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.NewsBannerPath = db.NewsPages.Find(1)?.NewsBannerPath;
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
