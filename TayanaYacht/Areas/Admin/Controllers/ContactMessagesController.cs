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
            var messages = db.ContactMessages.OrderByDescending(m => m.SubmittedAt).ToList();
            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkHandled(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var message = db.ContactMessages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            if (message.HandledAt != null)
            {
                TempData["Message"] = "這則留言已經處理過了";
                return RedirectToAction("Index");
            }

            message.HandledAt = DateTime.Now;
            db.SaveChanges();
            TempData["Message"] = "已標記為處理完成";
            return RedirectToAction("Index");
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var message = db.ContactMessages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }

            db.ContactMessages.Remove(message);
            db.SaveChanges();

            TempData["Message"] = "留言已刪除";
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
