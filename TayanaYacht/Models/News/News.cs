using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TayanaYacht.Models.News
{
    public class News
    {
        // Constructor
        public News()
        {
            NewsFiles = new HashSet<NewsFile>();
            NewsDetailImages = new HashSet<NewsDetailImage>();
        }
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "標題")]
        public string Title { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "摘要")]
        public string Summary { get; set; }

        [StringLength(255, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "新聞封面")]  
        public string NewsCoverPath { get; set; }

        [AllowHtml]
        [Display(Name = "內容")]
        public string ContentHtml { get; set; }

        //要在Migration中設定預設值，PublishDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()")
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "創建日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 告訴 EF 時間統一由資料庫產生
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "下架日期")]
        public DateTime? UnpublishedAt { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "刪除日期")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual ICollection<NewsFile> NewsFiles { get; set; }
        public virtual ICollection<NewsDetailImage> NewsDetailImages { get; set; }
    }
}