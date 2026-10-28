using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TayanaYacht.Models;
using TayanaYacht.Models.Contact;
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
            if (id == null || string.IsNullOrEmpty(direction))
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
            return Redirect(Url.Action("Index"));
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
            return View();
        }

        // POST: Admin/Yachts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SeriesName,ModelName,StatusText,IsLatest")] Yacht yacht)
        {
            if (ModelState.IsValid)
            {
                int maxOrder = db.Yachts.Select(y => y.DisplayOrder).OrderByDescending(y => y).FirstOrDefault();//DefaultIfEmpty(0).Max();
                int nextOrder = maxOrder + 1;
                yacht.DisplayOrder = nextOrder;
                yacht.UpdatedAt = DateTime.Now;
                db.Yachts.Add(yacht);
                db.SaveChanges();

                TempData["Message"] = "遊艇已建立，請繼續填寫內容、上傳圖片";
                return RedirectToAction("Edit", new { id = yacht.Id });
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
            if (yacht == null)
            {
                return HttpNotFound();
            }
            if (yacht.DeletedAt != null)
            {
                TempData["Message"] = "已下架的遊艇不能修改";
                return RedirectToAction("Index");
            }
            return View(yacht);
        }
        // POST: Admin/Yachts/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]

        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(id);
            // 遊艇下架後不能編輯
            if (yacht == null)
            {
                return HttpNotFound();
            }
            if (yacht.DeletedAt != null)
            {
                TempData["Message"] = "已下架的遊艇不能修改";
                return RedirectToAction("Index");
            }
            if (TryUpdateModel(yacht, new[] { "SeriesName", "ModelName", "StatusText", "OverviewHtml", "PrincipleDimensionHtml", "SpecificationHtml", "IsLatest", "IsPublished" }))
            {
                yacht.UpdatedAt = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(yacht);
        }


        #region Attachments
        // GET: Admin/Yachts/ManageAttachments/5
        public ActionResult ManageAttachments(int? id)
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
            ViewBag.Yacht = yacht;
            ViewBag.Files = db.YachtFiles.Where(f => f.YachtId == id).OrderBy(f => f.Id).ToList();
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
            TouchYacht(yachtId.Value);
            db.SaveChanges();
            return RedirectToAction("ManageAttachments", new { id = yachtId });
        }
        // POST: Admin/Yachts/AddPicture/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPicture([Bind(Include = "YachtId,PictureType,ImagePath")] YachtPicture yachtpicture)
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
                    return RedirectToAction("ManageAttachments", new { id = yachtpicture.YachtId });
                }
                db.YachtPictures.Add(yachtpicture);
                TouchYacht(yachtpicture.YachtId);
                db.SaveChanges();
                return RedirectToAction("ManageAttachments", new { id = yachtpicture.YachtId });
            }
            return RedirectToAction("ManageAttachments", new { id = yachtpicture.YachtId });
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
            TouchYacht(yachtId);
            db.SaveChanges();
            return RedirectToAction("ManageAttachments", new { id = yachtId });
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
                TouchYacht(yachtId);
            }
            db.SaveChanges();
            return RedirectToAction("ManageAttachments", new { id = yachtId });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: Admin/Yachts/DeManageAttachmentslete/Upload/5
        public ActionResult UploadFile(int? yachtId, string displayName, HttpPostedFileBase upload)
        {
            if (yachtId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yacht yacht = db.Yachts.Find(yachtId);
            if (yacht == null || yacht.DeletedAt != null)
            {
                return HttpNotFound();
            }
            if (upload == null || upload.ContentLength <= 0)
            {
                TempData["Message"] = "請選擇要上傳的檔案";
                return RedirectToAction("ManageAttachments", new { id = yachtId });
            }
            string extension = Path.GetExtension(upload.FileName).ToLower();
            if (extension != ".pdf")
            {
                TempData["Message"] = "只能上傳 PDF 檔案";
                return RedirectToAction("ManageAttachments", new { id = yachtId });
            }
            string fileOriginalName = string.IsNullOrWhiteSpace(displayName) ? Path.GetFileName(upload.FileName) : displayName.Trim();
            if (fileOriginalName.Length > 50)
            {
                TempData["Message"] = "檔案名稱不能超過 50 個字";
                return RedirectToAction("ManageAttachments", new { id = yachtId });
            }
            string storedFileName = Guid.NewGuid().ToString("N") + extension;
            string folder = Server.MapPath("~/Files/Yacht/" + yachtId + "/File/");
            Directory.CreateDirectory(folder);

            upload.SaveAs(Path.Combine(folder, storedFileName));
            var yachtFile = new YachtFile
            {
                YachtId = yachtId.Value,
                FileOriginalName = fileOriginalName,
                StoredFileName = storedFileName,
                FilePath = "/Files/Yacht/" + yachtId + "/File/" + storedFileName
            };
            db.YachtFiles.Add(yachtFile);
            TouchYacht(yachtFile.YachtId);
            db.SaveChanges();
            return RedirectToAction("ManageAttachments", new { id = yachtId });
        }
        // POST: Admin/Yachts/DeleteFile/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YachtFile yachtFile = db.YachtFiles.Find(id);
            if (yachtFile == null)
            {
                return HttpNotFound();
            }

            string physicalPath = Server.MapPath("~" + yachtFile.FilePath);

            db.YachtFiles.Remove(yachtFile);
            TouchYacht(yachtFile.YachtId);
            db.SaveChanges();

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            return RedirectToAction("ManageAttachments", new { id = yachtFile.YachtId });
        }

        #endregion

        #region Deleted and Restore
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
        #endregion
        #region Delete
        // GET: Admin/Yachts/Delete/5
        public ActionResult Delete(int? id)
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
            if (yacht.IsPublished)
            {
                TempData["Message"] = "已發布的遊艇無法下架，請先取消發布";
                return RedirectToAction("Index");
            }
            return View(yacht);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
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
            if (yacht.IsPublished)
            {
                TempData["Message"] = "已發布的遊艇無法下架，請先取消發布";
                return RedirectToAction("Index");
            }

            yacht.DeletedAt = DateTime.Now;
            db.SaveChanges();
            TempData["Message"] = "遊艇已下架";
            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private void TouchYacht(int yachtId)
        {
            var yacht = db.Yachts.Find(yachtId);
            if (yacht != null)
            {
                yacht.UpdatedAt = DateTime.Now;
            }
        }

        #endregion
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
