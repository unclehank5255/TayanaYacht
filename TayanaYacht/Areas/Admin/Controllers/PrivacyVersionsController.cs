using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.Contact;

namespace TayanaYacht.Areas.Admin.Controllers
{
    public class PrivacyVersionsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: Admin/PrivacyVersions
        public ActionResult Index()
        {
            var versions = db.PrivacyVersions.OrderByDescending(v => v.PublishedAt).ToList();

            int? activeVersionId = versions
                .Where(v => v.PublishedAt != null)
                .OrderByDescending(v => v.PublishedAt)
                .Select(v => (int?)v.Id)
                .FirstOrDefault();

            ViewBag.ActiveVersionId = activeVersionId;
            return View(versions);
        }

        // GET: Admin/PrivacyVersions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrivacyVersion privacyVersion = db.PrivacyVersions.Find(id);
            if (privacyVersion == null)
            {
                return HttpNotFound();
            }
            return View(privacyVersion);
        }

        // GET: Admin/PrivacyVersions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PrivacyVersions/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,ContentHtml")] PrivacyVersion privacyVersion)
        {
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                TempData["Message"] = "創建成功";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }

            return View(privacyVersion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(int? id)
        {
            var versionToPublish = db.PrivacyVersions.Find(id);
            if (versionToPublish == null)
            {
                return HttpNotFound();
            }
            if (versionToPublish.PublishedAt != null)
            {
                TempData["Message"] = "已發布的版本不能再次發佈";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }
            // Publish the selected version
            versionToPublish.PublishedAt = DateTime.Now;
            db.SaveChanges();
            TempData["Message"] = "發佈成功";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        // GET: Admin/PrivacyVersions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrivacyVersion privacyVersion = db.PrivacyVersions.Find(id);
            if (privacyVersion == null)
            {
                return HttpNotFound();
            }
            if (privacyVersion.PublishedAt != null)
            {
                TempData["Message"] = "已發布的版本不能修改";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }

            return View(privacyVersion);
        }

        // POST: Admin/PrivacyVersions/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var existing = db.PrivacyVersions.Find(id);
            if (existing == null)
                return HttpNotFound();

            if (existing.PublishedAt != null)
            {
                TempData["Message"] = "已發布的版本不能修改";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }

            if (TryUpdateModel(existing, new[] { "Title", "ContentHtml" }))
            {
                existing.ContentHtml = new HtmlSanitizer().Sanitize(existing.ContentHtml);
                if (string.IsNullOrEmpty(existing.ContentHtml))
                {
                    ModelState.AddModelError("ContentHtml", "<scripts>等字體會讓內容被吃掉，內容不能為空");
                    return View(existing);
                }
                db.SaveChanges();
                TempData["Message"] = "編輯成功";
                TempData["MessageType"] = "success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "編輯失敗";
            TempData["MessageType"] = "warning";
            return View(existing);
        }

        // GET: Admin/PrivacyVersions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrivacyVersion privacyVersion = db.PrivacyVersions.Find(id);
            if (privacyVersion == null)
            {
                return HttpNotFound();
            }
            if (privacyVersion.PublishedAt != null)
            {
                TempData["Message"] = "已發布的版本不能刪除";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "確定要刪除這條隱私版本嗎？";
            TempData["MessageType"] = "warning";
            return View(privacyVersion);
        }

        // POST: Admin/PrivacyVersions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrivacyVersion privacyVersion = db.PrivacyVersions.Find(id);
            if (privacyVersion == null)
            {
                return HttpNotFound();
            }
            if (privacyVersion.PublishedAt != null)
            {
                TempData["Message"] = "已發布的版本不能刪除";
                TempData["MessageType"] = "warning";
                return RedirectToAction("Index");
            }
            db.PrivacyVersions.Remove(privacyVersion);
            db.SaveChanges();
            TempData["Message"] = "刪除成功";
            TempData["MessageType"] = "success";
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
