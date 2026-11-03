using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TayanaYacht.Models.ViewModels
{
    public class DealerEditVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "國家")]
        public int CountryId { get; set; }

        [Required]
        [Display(Name = "經銷商名稱")]
        public string Name { get; set; }

        [Display(Name = "城市")]
        public string City { get; set; }

        public string CountryName { get; set; }

        public string DealerCoverPath { get; set; }

        [AllowHtml]
        public string ContentHtml { get; set; }

        [Display(Name = "顯示順序")]
        public int DisplayOrder { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}