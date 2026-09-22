using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYacht.Models.News
{
    public class NewsPage
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        // 設計成不主動給值Id固定為1，controller要設計成能抓null的模式，後台可以新增圖片
        //!!!Controller 要主動指定
        //Migiration加上這句Sql("ALTER TABLE dbo.NewsPages ADD CONSTRAINT CK_NewsPages_Id CHECK (Id = 1)");
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "新聞橫幅路徑不能超過 255 個字元")]
        [Display(Name = "新聞橫幅路徑")]
        public string NewsBannerPath { get; set; }
    }
}