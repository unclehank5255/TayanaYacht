using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TayanaYacht.Models.Contact
{
    public class Contact
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        // 設計成不主動給值Id固定為1，controller要設計成能抓null的模式，後台可以新增圖片
        //!!!Controller 要主動指定
        //Migiration加上這句Sql("ALTER TABLE dbo.Contacts ADD CONSTRAINT CK_Contacts_Id CHECK (Id = 1)");
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "聯絡橫幅路徑不能超過 255 個字元")]
        [Display(Name = "聯絡橫幅路徑")]
        public string ContactBannerPath { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "地圖圖片路徑不能超過 255 個字元")]
        [Display(Name = "地圖圖片路徑")]
        public string MapImagePath { get; set; }
        
        [Required(ErrorMessage = "內容是必填項")]
        [Display(Name = "內容")]
        public string ContentHtml { get; set; }

        [Display(Name = "隱私版本編號")]
        [Range(1, int.MaxValue, ErrorMessage = "選擇隱私政策版本")]
        public int PrivacyVersionId { get; set; }

        // Navigation property
        [ForeignKey("PrivacyVersionId")]
        virtual public PrivacyVersion PrivacyVersion { get; set; }
    }
}