using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.Yacht;
using TayanaYacht.Models.ViewModels;

namespace TayanaYacht.Controllers
{
    public class YachtsController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();
        [AllowAnonymous]
        // GET: Yachts
        public ActionResult Index(int? id,string tab)
        {

            var yachts = db.Yachts
                .Where(y => y.IsPublished && y.DeletedAt == null)
                .OrderByDescending(y => y.DisplayOrder)
                .ToList();


            Yacht currentYacht;
            if (id == null)
            {
                currentYacht = yachts.FirstOrDefault();
            }
            else
            {
                currentYacht = yachts.FirstOrDefault(y => y.Id == id);
            }

            if (currentYacht == null)
            {
                return HttpNotFound();
            }
            var vm = new YachtPageVM
            {
                Yachts = yachts,
                CurrentYacht = currentYacht,
                GalleryPictures = GetPicturesByType(currentYacht.Id, YachtPictureTypeEnum.Gallery),
                LayoutPictures = GetPicturesByType(currentYacht.Id, YachtPictureTypeEnum.Layout),
                ActiveTab = GetActiveTab(tab)
            };
           
            return View(vm);
        }
        private List<YachtPicture> GetPicturesByType(int yachtId, YachtPictureTypeEnum pictureType)
        {
            return db.YachtPictures
                .Where(p => p.YachtId == yachtId && p.PictureType == pictureType)
                .OrderBy(p => p.DisplayOrder)
                .ToList();
        }
        private static YachtTab GetActiveTab(string tab)
        {
            switch (tab?.ToLower())
            {
                case "layout":
                    return YachtTab.Layout;
                case "specification":
                    return YachtTab.Specification;
                default:
                    return YachtTab.Overview;
            }
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
