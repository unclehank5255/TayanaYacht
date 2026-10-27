using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.Contact;

namespace TayanaYacht.Areas.Admin.Controllers
{
    public class ContactsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();
        private const int ContactId = 1;

        public ActionResult Edit()
        {
            var contact = db.Contacts.Find(ContactId) ?? new Contact { Id = ContactId };
            return View(contact);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost()
        {
            var contact = db.Contacts.Find(ContactId);
            bool isNew = contact == null;
            if (isNew)
            {
                contact = new Contact { Id = ContactId };
            }

            if (!TryUpdateModel(contact, new[] { "ContactBannerPath", "ContentHtml", "MapEmbedUrl" }))
            {
                return View(contact);
            }

            if (isNew)
            {
                db.Contacts.Add(contact);
            }
            db.SaveChanges();

            TempData["Message"] = "聯絡頁設定已儲存";
            return RedirectToAction("Edit");
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