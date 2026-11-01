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
using TayanaYacht.Models.ViewModels;

namespace TayanaYacht.Controllers
{
    public class NewsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: News
        public ActionResult Index(int page = 1)
        {
            const int pageSize = 5;
            page = (page < 1) ? 1 : page;
            var query = db.News.Where(n => n.DeletedAt == null && n.UnpublishedAt == null);

            int totalCount = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            int startPage;
            int endPage;
            if(totalPages <= 5)
            {
                startPage = 1;
                endPage = totalPages;
            }
            else
            {
                startPage = page - 2;
                if (startPage < 1)
                {
                    startPage = 1;
                }
                endPage = startPage + 4;
                if (endPage > totalPages)
                {
                    endPage = totalPages;
                    startPage = endPage - 4;
                }
            }
            var vm = new NewsVM()
            {
                NewsBannerPath = db.NewsPages.Find(1)?.NewsBannerPath,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = totalPages,
                StartPage = startPage,
                EndPage = endPage,
                NewsList = query.
                OrderByDescending(n => n.PublishDate).
                ThenByDescending(n => n.Id).Skip((page - 1)*pageSize)
                .Take(pageSize).ToList()
            };
            
            return View(vm);
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
            if (news == null || news.DeletedAt != null || news.UnpublishedAt != null)
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
