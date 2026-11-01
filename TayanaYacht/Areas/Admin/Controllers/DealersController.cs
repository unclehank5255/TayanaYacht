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
using TayanaYacht.Models.ViewModels;


namespace TayanaYacht.Areas.Admin.Controllers
{
    public class DealersController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: Dealers
        public ActionResult Index()
        {
            ViewBag.BannerPath = db.DealerPages.Find(1)?.DealerBannerPath;
            return View(db.Dealers.Include(d => d.Country).OrderBy(d=>d.DisplayOrder).ThenBy(d=>d.Id).ToList());
        }

        // GET: Dealers/Create
        public ActionResult Create()
        {
            TempData["Message"] = "先填國家編號 城市 經銷商名稱 和顯示順序，建立後會進入編輯頁，再填寫內容、上傳圖片。新增新聞預設為<strong>未上架</strong>。";
            TempData["MessageType"] = "info";
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
                dealer.UnpublishedAt = DateTime.Now;
                db.Dealers.Add(dealer);
                db.SaveChanges();
                TempData["Message"] = "經銷商已建立，請繼續填寫內容、上傳圖片";
                TempData["MessageType"] = "success";
                return RedirectToAction("Edit", new { id = dealer.Id });
            }

            return View(dealer);
        }

        // GET: Dealers/Edit/5
        public ActionResult Edit(int? id)
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
            var dealerEditVM = new DealerEditVM
            {
                Id = dealer.Id,
                CountryId = dealer.CountryId,
                Name = dealer.Name,
                City = dealer.City,
                CountryName = dealer.Country.Name,
                DealerCoverPath = dealer.DealerCoverPath,
                ContentHtml = dealer.ContentHtml,
                DisplayOrder = dealer.DisplayOrder,
                CountryList = new SelectList(db.Countries.OrderBy(c => c.DisplayOrder), "Id", "Name")
            };
            return View(dealerEditVM);
        }

        // POST: Dealers/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DealerEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.CountryList = new SelectList(db.Countries.OrderBy(c => c.DisplayOrder), "Id", "Name");
                vm.CountryName = db.Countries.Find(vm.CountryId)?.Name;
                return View(vm);
            }

            Dealer dealer = db.Dealers.Find(vm.Id);
            if (dealer == null)
            {
                return HttpNotFound();
            }

            dealer.CountryId = vm.CountryId;
            dealer.Name = vm.Name;
            dealer.City = vm.City;
            dealer.DealerCoverPath = vm.DealerCoverPath;
            dealer.ContentHtml = vm.ContentHtml;
            dealer.DisplayOrder = vm.DisplayOrder;

            db.SaveChanges();
            TempData["Message"] = "編輯成功";
            TempData["MessageType"] = "success";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TogglePublish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dealer = db.Dealers.Find(id);
            if (dealer == null)
            {
                return HttpNotFound();
            }
            if (dealer.UnpublishedAt != null)
            {
                if (string.IsNullOrWhiteSpace(dealer.DealerCoverPath))
                {
                    TempData["Message"] = "無法上架，請先設定封面圖片";
                    TempData["MessageType"] = "warning";
                    return RedirectToAction("Index");
                }
                dealer.UnpublishedAt = null;
            }
            else
            {
                dealer.UnpublishedAt = DateTime.Now;
            }

            TempData["Message"] = "狀態更新成功";
            TempData["MessageType"] = "success";
            db.SaveChanges();
            return RedirectToAction("Index");
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
            TempData["Message"] = "封面更新成功";
            TempData["MessageType"] = "success";
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
            TempData["Message"] = "確定要刪除這間經銷商嗎？刪除後前台不會顯示，後台列表也看不到。\r\n";
            TempData["MessageType"] = "warning";

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
