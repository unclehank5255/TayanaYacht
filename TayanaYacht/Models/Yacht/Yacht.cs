using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [Display(Name = "系列")]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        public string SeriesName { get; set; }

        [Required]
        [Display(Name = "型號")]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        public string ModelName { get; set; }


        [Display(Name = "狀態")]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        public string StatusText { get; set; } // 可null，更高版本才需要在string加?

        [Display(Name = "遊艇封面")]
        [StringLength(255, ErrorMessage = "{0}不能超過 {1} 個字元")]
        public string YachtBannerPath { get; set; }

        [AllowHtml]
        [Display(Name = "概覽OverviewHtml")]
        public string OverviewHtml { get; set; }

        [AllowHtml]
        [Display(Name = "主要尺寸PrincipleDimension")]
        public string PrincipleDimensionHtml { get; set; }

        [AllowHtml]
        [Display(Name = "規格Specification")]
        public string SpecificationHtml { get; set; }

        [Display(Name = "排序")]
        public int DisplayOrder { get; set; }

        [Display(Name = "下架時間")]
        public DateTime? DeletedAt { get; set; }

        [Display(Name = "最新款")]
        public bool IsLatest { get; set; }

        [Display(Name = "已發布")]
        public bool IsPublished { get; set; }

        // Navigation properties
        public virtual ICollection<YachtPicture> YachtPictures { get; set; }
        public virtual ICollection<YachtFile> YachtFiles { get; set; }
        public virtual ICollection<Contact.ContactMessage> ContactMessages { get; set; }
    }
}