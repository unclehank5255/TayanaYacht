using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.News;
using TayanaYacht.Models.Yacht;

namespace TayanaYacht.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: News
        public ActionResult Index()
        {
            ViewBag.BannerPath = db.NewsPages.Find(1)?.NewsBannerPath;
            return View(db.News.Where(n => n.DeletedAt == null).OrderByDescending(n => n.CreatedAt).ToList());
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

            if (news.UnpublishedAt != null)
            {
                if (string.IsNullOrWhiteSpace(news.ContentHtml))
                {
                    TempData["Message"] = "無法上架，請先編輯內容";
                    return RedirectToAction("Index");
                }
                if (string.IsNullOrWhiteSpace(news.NewsCoverPath))
                {
                    TempData["Message"] = "無法上架，請先設定封面圖片";
                    return RedirectToAction("Index");
                }
                news.UnpublishedAt = null;
            }
            else
            {
                news.UnpublishedAt = DateTime.Now;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
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
                news.UnpublishedAt = DateTime.Now;
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
                if (existingNews == null || existingNews.DeletedAt != null)
                {
                    return HttpNotFound();
                }
                existingNews.Title = news.Title;
                existingNews.Summary = news.Summary;
                existingNews.ContentHtml = news.ContentHtml;
                existingNews.NewsCoverPath = news.NewsCoverPath;
                existingNews.UpdatedAt = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = existingNews.Id });
            }
            return View(news);
        }

        //POST: Admin/News/UpdatePageBanner/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePageBanner(string bannerPath)
        {

            NewsPage newsPage = db.NewsPages.Find(1);
            if (newsPage == null)
            {
                newsPage = new NewsPage { Id = 1 };
                db.NewsPages.Add(newsPage);
            }
            newsPage.NewsBannerPath = bannerPath;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ManageFiles(int? id)
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

        // POST: Admin/News/Upload/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(int? id, string displayName, HttpPostedFileBase upload)
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
            if (upload == null || upload.ContentLength <= 0)
            {
                TempData["Message"] = "請選擇要上傳的檔案";
                return RedirectToAction("ManageFiles", new { id = news.Id });
            }
            string extension = Path.GetExtension(upload.FileName).ToLower();
            if (extension != ".pdf")
            {
                TempData["Message"] = "只能上傳 PDF 檔案";
                return RedirectToAction("ManageFiles", new { id = news.Id });
            }
            string displayFileName = string.IsNullOrWhiteSpace(displayName) ? Path.GetFileName(upload.FileName) : displayName.Trim();
            if (displayFileName.Length > 50)
            {
                TempData["Message"] = "檔案名稱不能超過 50 個字";
                return RedirectToAction("ManageFiles", new { id = news.Id });
            }
            string storedFileName = Guid.NewGuid().ToString("N") + extension;
            string folder = Server.MapPath("~/Files/News/" + news.Id + "/File/");
            Directory.CreateDirectory(folder);

            upload.SaveAs(Path.Combine(folder, storedFileName));
            var newsFile = new NewsFile
            {
                NewsId = news.Id,
                DisplayFileName = displayFileName,
                StoredFileName = storedFileName,
                FilePath = "/Files/News/" + news.Id + "/File/" + storedFileName
            };
            db.NewsFiles.Add(newsFile);
            db.SaveChanges();
            return RedirectToAction("ManageFiles", new { id = news.Id });
        }
        // POST: Admin/News/DeleteFile/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFile newsFile = db.NewsFiles.Find(id);
            if (newsFile == null)
            {
                return HttpNotFound();
            }

            string physicalPath = Server.MapPath("~" + newsFile.FilePath);

            db.NewsFiles.Remove(newsFile);
            db.SaveChanges();

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            return Redirect(Url.Action("Edit", new { id = newsFile.NewsId }) + "#files");
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
