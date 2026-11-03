using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TayanaYacht.Models.Contact;

namespace TayanaYacht.Models.ViewModels
{
    public class ContactPageVM
    {
        [Required(ErrorMessage = "{0}是必填項")]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [EmailAddress]
        [StringLength(254, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [StringLength(30, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "電話")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "請選擇{0}")]
        [Display(Name = "國家")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [StringLength(100, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "評論")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "請選擇{0}")]
        [Display(Name = "遊艇")]
        public int? YachtId { get; set; }
        public IEnumerable<SelectListItem> YachtList { get; set; }
        public TayanaYacht.Models.Contact.Contact ContactPage { get; set; }
        public PrivacyVersion Privacy { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "請先同意隱私權政策")]
        public bool AgreePrivacy { get; set; }
    }
}