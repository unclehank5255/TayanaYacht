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

namespace TayanaYacht.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: News
        public ActionResult Index()
        {
            return View(db.News.Where(n => n.DeletedAt == null).ToList());
        }
        // POST: Publish/Unpublish News
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TogglePublish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var news = db.News.Find(id);
            if (news == null || news.DeletedAt != null)
            {
                return HttpNotFound();
            }
            news.IsPublished = !news.IsPublished;
            news.UnpublishedAt = news.IsPublished ? (DateTime?)null : DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null || news.DeletedAt != null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Summary,ContentHtml")] News news)
        {
            if (ModelState.IsValid)
            {
                news.UpdatedAt = DateTime.Now;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null || news.DeletedAt != null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        //news.UpdatedAt = DateTime.Now;
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Summary,NewsCoverPath,ContentHtml")] News news)
        {
            if (ModelState.IsValid)
            {
                News existingNews = db.News.Find(news.Id);
                if (existingNews == null || news.DeletedAt != null)
                {
                    return HttpNotFound();
                }
                existingNews.Title = news.Title;
                existingNews.Summary = news.Summary;
                existingNews.ContentHtml = news.ContentHtml;
                existingNews.NewsCoverPath = news.NewsCoverPath;
                existingNews.UpdatedAt = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null || news.DeletedAt != null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null || news.DeletedAt != null)
            {
                return HttpNotFound();
            }
            news.DeletedAt = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
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
