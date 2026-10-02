using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYacht.Models.News
{
    public class NewsFile
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "新聞編號")]
        [Range(1, int.MaxValue, ErrorMessage = "請選擇新聞 ID")]
        public int NewsId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "檔案名稱不能超過 255 個字元")]
        [Display(Name = "原始檔案名稱")]
        public string OriginalFileName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "儲存檔案名稱不能超過 255 個字元")]
        [Display(Name = "儲存檔案名稱")]
        public string StoredFileName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "檔案路徑不能超過 255 個字元")]
        [Display(Name = "檔案路徑")]
        public string FilePath { get; set; }

        // Navigation property
        [ForeignKey("NewsId")]
        virtual public News News { get; set; }
    }
}