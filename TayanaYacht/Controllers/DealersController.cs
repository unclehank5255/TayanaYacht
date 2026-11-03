using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TayanaYacht.Models;
using TayanaYacht.Models.Dealer;
using TayanaYacht.Models.ViewModels;

namespace TayanaYacht.Controllers
{
    [AllowAnonymous]
    public class DealersController : Controller
    {
        private TayanaDbContext db = new TayanaDbContext();

        public ActionResult Index(int? id)
        {
            var countries = db.Countries
                .Where(c => db.Dealers.Any(d => d.CountryId == c.Id && d.UnpublishedAt == null))
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Id)
                .ToList();

            var current = countries.FirstOrDefault(c => c.Id == id) ?? countries.FirstOrDefault();

            var vm = new DealerPageVM
            {
                Countries = countries,
                CurrentCountry = current,
                Dealers = current == null
                    ? new List<Dealer>()
                    : db.Dealers
                        .Where(d => d.CountryId == current.Id && d.UnpublishedAt == null)
                        .OrderBy(d => d.DisplayOrder)
                        .ThenBy(d => d.Id)
                        .ToList(),
                DealerBannerPath = db.DealerPages.Find(1)?.DealerBannerPath
            };

            return View(vm);
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