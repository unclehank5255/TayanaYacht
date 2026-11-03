using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.ViewModels;
using TayanaYacht.Models.Contact;

namespace TayanaYacht.Controllers
{
    public class ContactController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var vm = new ContactPageVM();
            FillDisplayData(vm);
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public ActionResult IndexPost(ContactPageVM vm)
        {
            if (!ModelState.IsValid)
            {
                FillDisplayData(vm);
                return View(vm);
            }
            var yacht = db.Yachts.Where(y => y.Id == vm.YachtId && y.IsPublished == true && y.DeletedAt == null).FirstOrDefault();
            var country = db.Countries.FirstOrDefault(c => c.Id == vm.CountryId && db.Dealers.Any(d => d.CountryId == c.Id && d.UnpublishedAt == null));
            var privacy = GetPrivacyVersion();
            if (country == null)
            {
                ModelState.AddModelError("CountryId", "請選擇有效的國家");
            }
            if (yacht == null)
            {
                ModelState.AddModelError("YachtId", "請選擇有效的遊艇");
            }
            if (privacy == null)
            {
                ModelState.AddModelError("Privacy", "隱私權政策尚未設定");
            }
            if (!ModelState.IsValid)
            {
                FillDisplayData(vm);
                return View(vm);
            }
            var contactMessage = new ContactMessage
            {
                Name = vm.Name,
                Email = vm.Email,
                Phone = vm.Phone,
                CountryId = country.Id,
                CountryNameSnapshot = country.Name,
                YachtId = vm.YachtId.Value,
                Comment = vm.Comment,
                YachtNameSnapshot = yacht.SeriesName + " " + yacht.ModelName,
                PrivacyVersionId = privacy.Id,
            };
            db.ContactMessages.Add(contactMessage);
            db.SaveChanges();

            TempData["Message"] = "您的訊息已送出，我們將盡快與您聯繫。";
            return RedirectToAction("Index");
        }

        private void FillDisplayData(ContactPageVM vm)
        {
            vm.ContactPage = db.Contacts.Find(1);
            vm.Privacy = GetPrivacyVersion();
            vm.YachtList = (IEnumerable<SelectListItem>)db.Yachts.Where(y => y.IsPublished == true && y.DeletedAt == null).OrderByDescending(y => y.DisplayOrder).ThenByDescending(y => y.Id).ToList().Select(y => new SelectListItem
            {
                Value = y.Id.ToString(),
                Text = y.SeriesName + " " + y.ModelName
            });
            vm.CountryList = db.Countries.Where(c => db.Dealers.Any(d => d.CountryId == c.Id && d.UnpublishedAt == null)).OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id).ToList().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }
        private PrivacyVersion GetPrivacyVersion()
        {
            return db.PrivacyVersions.OrderByDescending(p => p.PublishedAt).ThenByDescending(p => p.Id).FirstOrDefault();
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