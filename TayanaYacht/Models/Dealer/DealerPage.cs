using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace TayanaYacht.Models.Dealer
{
    public class DealerPage
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        // 設計成不主動給值Id固定為1，controller要設計成能抓null的模式，後台可以新增圖片
        //!!!Controller 要主動指定
        //Migiration加上這句Sql("ALTER TABLE dbo.DealerPages ADD CONSTRAINT CK_DealerPages_Id CHECK (Id = 1)");
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "經銷商橫幅路徑不能超過 255 個字元")]
        [Display(Name = "經銷商橫幅路徑")]
        public string DealerBannerPath { get; set; }
    }
}