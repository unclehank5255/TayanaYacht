using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TayanaYacht.Models;
using TayanaYacht.Models.Yacht;
using static TayanaYacht.Models.ImageDefaults;

namespace TayanaYacht.Areas.Admin.Controllers
{
    public class YachtsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        // GET: Admin/Yachts
        public ActionResult Index()
        {
            return View(db.Yachts.Where(y => y.DeletedAt == null).OrderByDescending(y => y.DisplayOrder).ToList());
        }

        // POST: Admin/Yachts/MoveYachts/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveYachts(int? id, string direction)
        {
            if (id == null || string.IsNullOrEmpty(direction) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null || yacht.DeletedAt != null)
            {
                return HttpNotFound();
            }
            IQueryable<Yacht> neighborQuery = db.Yachts.Where(y => y.DeletedAt == null);
            Yacht neighbor = null;
            if (direction == "up")//先判斷區間 再用排列找到第一個數，資料表大於當前選中 且由小排列到大
            {
                neighbor = neighborQuery.Where(y => y.DisplayOrder > yacht.DisplayOrder).OrderBy(y => y.DisplayOrder).FirstOrDefault();
            }
            else if (direction == "down")
            {
                neighbor = neighborQuery.Where(y => y.DisplayOrder < yacht.DisplayOrder).OrderByDescending(y => y.DisplayOrder).FirstOrDefault();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int temp;
            if (neighbor != null)
            {
                temp = yacht.DisplayOrder;
                yacht.DisplayOrder = neighbor.DisplayOrder;
                neighbor.DisplayOrder = temp;
            }
            db.SaveChanges();
            return Redirect(Url.Action("Index")+"#"+yacht.Id);
        }

        // GET: Admin/Yachts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null)
            {
                return HttpNotFound();
            }
            return View(yacht);
        }

        // GET: Admin/Yachts/Create
        public ActionResult Create()
        {
            Yacht yacht = new Yacht();
            yacht.YachtBannerPath = ImageDefaults.Picture;
            yacht.OverviewHtml = "<h2>遊艇介紹</h2><p>請修改內容</p>";
            yacht.PrincipleDimensionHtml = "<h2>主要尺寸</h2><p>請修改內容</p>";
            yacht.SpecificationHtml = "<h2>規格</h2><p>請修改內容</p>";
            return View(yacht);
        }

        // POST: Admin/Yachts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SeriesName,ModelName,StatusText,OverviewHtml,PrincipleDimensionHtml,SpecificationHtml,,IsLatest")] Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                int maxOrder = db.Yachts.Select(y => y.DisplayOrder).OrderByDescending(y => y).FirstOrDefault();//DefaultIfEmpty(0).Max();
                int nextOrder = maxOrder + 1;
                yacht.DisplayOrder = nextOrder;
                db.Yachts.Add(yacht);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(yacht);
        }

        // GET: Admin/Yachts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            // 遊艇下架後不能編輯
            if (yacht == null || yacht.DeletedAt != null)
            {
                return HttpNotFound();
            }
            return View(yacht);
        }

        // POST: Admin/Yachts/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SeriesName,ModelName,StatusText,YachtBannerPath,OverviewHtml,PrincipleDimensionHtml,SpecificationHtml,DisplayOrder,DeletedAt,IsLatest")] Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yacht).State = EntityState.Modified;
                db.Entry(yacht).Property(y => y.YachtBannerPath).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yacht);
        }

        // GET: Admin/Yachts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);

            return View(yacht);
        }

        // POST: Admin/Yachts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yacht yacht = db.Yachts.Find(id);
            if(yacht.IsPublished)
            {
                TempData["Message"] = "已發佈的遊艇無法刪除，請先取消發佈";
            }
            if (yacht == null)
            {
                return HttpNotFound();
            }
            yacht.DeletedAt = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Yachts/ManagePictures/5
        public ActionResult ManagePictures(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null || yacht.DeletedAt != null)
            {
                return HttpNotFound();
            }
            ViewBag.YachtBannerPath = yacht.YachtBannerPath ?? ImageDefaults.Picture;
            var pictures = db.YachtPictures.Where(p => p.YachtId == id).OrderBy(p => p.DisplayOrder).ToList();
            ViewBag.YachtId = id;
            ViewBag.YachtName = $"{yacht.SeriesName} {yacht.ModelName}";
            return View(pictures);
        }
        //POST: Admin/Yachts/UpdateBanner/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBanner(int? yachtId, string bannerPath)
        {
            if (yachtId == null || string.IsNullOrEmpty(bannerPath))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var yacht = db.Yachts.Find(yachtId);
            if (yacht == null)
            {
                return HttpNotFound();
            }

            yacht.YachtBannerPath = bannerPath;
            db.SaveChanges();
            return RedirectToAction("ManagePictures", new { id = yachtId });
        }
        // POST: Admin/Yachts/ManagePictures/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManagePictures([Bind(Include = "YachtId,PictureType,ImagePath")] YachtPicture yachtpicture)
        {
            if (ModelState.IsValid)
            {
                int maxOrder = db.YachtPictures.Where(y => y.YachtId == yachtpicture.YachtId && y.PictureType == yachtpicture.PictureType).Select(p => p.DisplayOrder).DefaultIfEmpty(0).Max();
                int nextOrder = maxOrder + 1;
                yachtpicture.DisplayOrder = nextOrder;
                //限制只能抓三張
                if (db.YachtPictures.Where(y => y.YachtId == yachtpicture.YachtId
                && y.PictureType == yachtpicture.PictureType).Count() >= 3)
                {
                    TempData["Message"] = "每種類型最多只能新增三張圖片";
                    return RedirectToAction("ManagePictures", new { id = yachtpicture.YachtId });
                }
                db.YachtPictures.Add(yachtpicture);
                db.SaveChanges();
                return RedirectToAction("ManagePictures", new { id = yachtpicture.YachtId });
            }
            return RedirectToAction("ManagePictures", new { id = yachtpicture.YachtId });
        }
        // POST: Admin/Yachts/DeletePicture/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePicture(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YachtPicture yachtpicture = db.YachtPictures.Find(id);
            if (yachtpicture == null)
            {
                return HttpNotFound();
            }
            int yachtId = yachtpicture.YachtId;
            db.YachtPictures.Remove(yachtpicture);
            db.SaveChanges();
            return RedirectToAction("ManagePictures", new { id = yachtId });
        }

        // POST: Admin/Yachts/MovePicture/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovePicture(int? id, string direction)
        {
            if (id == null || string.IsNullOrEmpty(direction))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YachtPicture yachtpicture = db.YachtPictures.Find(id);
            if (yachtpicture == null)
            {
                return HttpNotFound();
            }

            int yachtId = yachtpicture.YachtId;
            IQueryable<YachtPicture> neighborQuery = db.YachtPictures
            .Where(y => y.YachtId == yachtpicture.YachtId
            && y.PictureType == yachtpicture.PictureType);
            YachtPicture neighbor = null;
            if (direction == "up")//先判斷區間 再用排列找到第一個數
            {
                neighbor = neighborQuery.Where(y => y.DisplayOrder < yachtpicture.DisplayOrder).OrderByDescending(p => p.DisplayOrder).FirstOrDefault();
            }
            else if (direction == "down")
            {
                neighbor = neighborQuery.Where(y => y.DisplayOrder > yachtpicture.DisplayOrder).OrderBy(p => p.DisplayOrder).FirstOrDefault();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);//資料表大於當前選中 且由小排列到大
            }
            int temp;
            if (neighbor != null)
            {
                temp = yachtpicture.DisplayOrder;
                yachtpicture.DisplayOrder = neighbor.DisplayOrder;
                neighbor.DisplayOrder = temp;
            }
            db.SaveChanges();
            //根據id區域轉址
            if (yachtpicture.PictureType.ToString().ToLower() == "layout")
            {
                return Redirect(Url.Action("ManagePictures", new { id = yachtId }) + "#layout");
            }
            else
            {
                return RedirectToAction("ManagePictures", new { id = yachtId });
            }
        }
        // GET: Admin/Yachts/Deleted
        [HttpGet]
        public ActionResult Deleted()
        {
            return View(db.Yachts.Where(y => y.DeletedAt != null).OrderByDescending(y => y.DeletedAt).ToList());
        }
        // POST: Admin/Yachts/Restore/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            if (yacht == null)
            {
                return HttpNotFound();
            }
            yacht.DeletedAt = null;
            db.SaveChanges();
            return RedirectToAction("Deleted");
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
