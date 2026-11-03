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
    public class ContactMessagesController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: ContactMessages
        public ActionResult Index()
        {
            var contactMessages = db.ContactMessages.Include(c => c.PrivacyVersion).Include(c => c.Yacht);
            return View(contactMessages.ToList());
        }

        // GET: ContactMessages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactMessage contactMessage = db.ContactMessages.Find(id);
            if (contactMessage == null)
            {
                return HttpNotFound();
            }
            return View(contactMessage);
        }

        // GET: ContactMessages/Create
        public ActionResult Create()
        {
            ViewBag.PrivacyVersionId = new SelectList(db.PrivacyVersions, "Id", "Version");
            ViewBag.YachtId = new SelectList(db.Yachts, "Id", "SeriesName");
            return View();
        }

        // POST: ContactMessages/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Phone,Country,YachtId,YachtNameSnapshot,Comment,AcceptedPrivacy,PrivacyVersionId,SubmittedAt")] ContactMessage contactMessage)
        {
            if (ModelState.IsValid)
            {
                db.ContactMessages.Add(contactMessage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PrivacyVersionId = new SelectList(db.PrivacyVersions, "Id", "Version", contactMessage.PrivacyVersionId);
            ViewBag.YachtId = new SelectList(db.Yachts, "Id", "SeriesName", contactMessage.YachtId);
            return View(contactMessage);
        }

        // GET: ContactMessages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactMessage contactMessage = db.ContactMessages.Find(id);
            if (contactMessage == null)
            {
                return HttpNotFound();
            }
            ViewBag.PrivacyVersionId = new SelectList(db.PrivacyVersions, "Id", "Version", contactMessage.PrivacyVersionId);
            ViewBag.YachtId = new SelectList(db.Yachts, "Id", "SeriesName", contactMessage.YachtId);
            return View(contactMessage);
        }

        // POST: ContactMessages/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Phone,Country,YachtId,YachtNameSnapshot,Comment,AcceptedPrivacy,PrivacyVersionId,SubmittedAt")] ContactMessage contactMessage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactMessage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PrivacyVersionId = new SelectList(db.PrivacyVersions, "Id", "Version", contactMessage.PrivacyVersionId);
            ViewBag.YachtId = new SelectList(db.Yachts, "Id", "SeriesName", contactMessage.YachtId);
            return View(contactMessage);
        }

        // GET: ContactMessages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactMessage contactMessage = db.ContactMessages.Find(id);
            if (contactMessage == null)
            {
                return HttpNotFound();
            }
            return View(contactMessage);
        }

        // POST: ContactMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactMessage contactMessage = db.ContactMessages.Find(id);
            db.ContactMessages.Remove(contactMessage);
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
