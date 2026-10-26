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
            return View(db.PrivacyVersions.ToList());
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
                db.PrivacyVersions.Add(privacyVersion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(privacyVersion);
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
            return View(privacyVersion);
        }

        // POST: Admin/PrivacyVersions/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ContentHtml,CreatedAt")] PrivacyVersion privacyVersion)
        {
            var existing = db.PrivacyVersions.Find(privacyVersion.Id);
            if(existing == null)
            {
                return HttpNotFound();
            }
            if (existing.PublishedAt != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Entry(privacyVersion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(privacyVersion);
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
            return View(privacyVersion);
        }

        // POST: Admin/PrivacyVersions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrivacyVersion privacyVersion = db.PrivacyVersions.Find(id);
            db.PrivacyVersions.Remove(privacyVersion);
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
