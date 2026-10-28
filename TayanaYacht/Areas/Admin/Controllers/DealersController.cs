using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.Dealer;
using TayanaYacht.Models.News;
using TayanaYacht.Models.Yacht;


namespace TayanaYacht.Areas.Admin.Controllers
{
    public class DealersController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: Dealers
        public ActionResult Index()
        {
            ViewBag.BannerPath = db.DealerPages.Find(1)?.DealerBannerPath;
            return View(db.Dealers.Include(d => d.Country).ToList());
        }

        // GET: Dealers/Create
        public ActionResult Create()
        {
            ViewBag.CountryList = new SelectList(db.Countries.OrderBy(c => c.DisplayOrder), "Id", "Name");
            return View();
        }

        // POST: Dealers/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryId,Name,City,DisplayOrder")] Dealer dealer)
        {
            ViewBag.CountryList = new SelectList(db.Countries.OrderBy(c => c.DisplayOrder), "Id", "Name");
            if (ModelState.IsValid)
            {
                db.Dealers.Add(dealer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dealer);
        }

        // GET: Dealers/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CountryList = new SelectList(db.Countries.OrderBy(c => c.DisplayOrder), "Id", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Dealers/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CountryId,Name,City,DealerCoverPath,ContentHtml,DisplayOrder")] Dealer dealer)
        {
            ViewBag.CountryList = new SelectList(db.Countries.OrderBy(c => c.DisplayOrder), "Id", "Name");
            if (ModelState.IsValid)
            {
                db.Entry(dealer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dealer);
        }
        //POST: Admin/News/UpdatePageBanner/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePageBanner(string bannerPath)
        {

            DealerPage dealerPage = db.DealerPages.Find(1);
            if (dealerPage == null)
            {
                dealerPage = new DealerPage { Id = 1 };
                db.DealerPages.Add(dealerPage);
            }
            dealerPage.DealerBannerPath = bannerPath;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Admin/Dealers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Include(d => d.Country).FirstOrDefault(d => d.Id == id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            return View(dealer);
        }

        // POST: Admin/Dealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dealer dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            db.Dealers.Remove(dealer);
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
