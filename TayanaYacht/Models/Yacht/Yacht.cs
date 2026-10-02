using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYacht.Models.Yacht
{
    public class Yacht
    {   
        // Constructor to initialize the navigation properties
        public Yacht()
        {
            YachtPictures = new HashSet<YachtPicture>();
            YachtFiles = new HashSet<YachtFile>();
            ContactMessages = new HashSet<Contact.ContactMessage>();
        }
        //因為有OverviewHtml、ContentHtml、SpecificationHtml所以後台要注意不能輸入這個<script>...</script>
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "系列名稱不能超過 50 個字元")]
        [Display(Name = "系列名稱")]
        public string SeriesName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "型號名稱不能超過 50 個字元")]
        [Display(Name = "型號名稱")]
        public string ModelName { get; set; }


        [Display(Name = "狀態文字")]
        [StringLength(50, ErrorMessage = "狀態文字不能超過 50 個字元")]
        public string StatusText { get; set; } // 可null，更高版本才需要在string加?

        [Required]
        [StringLength(255, ErrorMessage = " yacht標誌路徑不能超過 255 個字元")]
        [Display(Name = " yacht標誌路徑")]
        public string YachtBannerPath { get; set; }

        [Required]
        [Display(Name = "概覽")]
        public string OverviewHtml { get; set; }

        [Required]
        [Display(Name = "主要尺寸")]
        public string PrincipleDimensionHtml { get; set; }

        [Required]
        [Display(Name = "規格")]
        public string SpecificationHtml { get; set; }

        [Display(Name = "顯示順序")]
        public int DisplayOrder { get; set; }

        [Display(Name = "下架時間")]
        public DateTime? DeletedAt { get; set; }

        [Display(Name = "是否為最新")]
        public bool IsLatest { get; set; }

        // Navigation properties
        public virtual ICollection<YachtPicture> YachtPictures { get; set; }
        public virtual ICollection<YachtFile> YachtFiles { get; set; }
        public virtual ICollection<Contact.ContactMessage> ContactMessages { get; set; }
    }
}