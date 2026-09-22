using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(25, ErrorMessage = "標題不能超過 25 個字元")]
        [Display(Name = "標題")]
        public string Title { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "摘要不能超過 50 個字元")]
        [Display(Name = "摘要")]
        public string Summary { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "新聞封面路徑不能超過 255 個字元")]
        [Display(Name = "新聞封面路徑")]  
        public string NewsCoverPath { get; set; }

        [Required(ErrorMessage = "內容是必填項")]
        [Display(Name = "內容")]
        public string ContentHtml { get; set; }

        //要在Migration中設定預設值，PublishDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()")

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "發布日期")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // 告訴 EF 時間統一由資料庫產生
        public DateTime PublishDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "刪除日期")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual ICollection<NewsFile> NewsFiles { get; set; }
        public virtual ICollection<NewsDetailImage> NewsDetailImages { get; set; }
    }
}