using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYacht.Models.News
{
    public class NewsDetailImage
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "新聞編號")]
        [Range(1, int.MaxValue, ErrorMessage = "{0}必須在 {1} 到 {2} 之間")]
        public int NewsId { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [Display(Name = "新聞內文圖片路徑")]
        [StringLength(255, ErrorMessage = "{0}不能超過 {1} 個字元")]
        public string DetailImagePath { get; set; }

        // Navigation property
        [ForeignKey("NewsId")]
        virtual public News News { get; set; }
    }
}