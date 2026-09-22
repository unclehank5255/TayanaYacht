using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYacht.Models.Contact
{
    public class ContactMessage
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "姓名是必填項")]
        [StringLength(50, ErrorMessage = "姓名不能超過 50 個字元")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "電子郵件是必填項")]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "電子郵件不能超過 50 個字元")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Required(ErrorMessage = "電話是必填項")]
        [StringLength(30, ErrorMessage = "電話不能超過 30 個字元")]
        [Display(Name = "電話")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "國家是必填項")]
        [StringLength(60, ErrorMessage = "國家不能超過 60 個字元")]
        [Display(Name = "國家")]
        public string Country { get; set; }

        [Display(Name = "游艇編號")]
        [Required(ErrorMessage = "請選擇遊艇")]
        public int? YachtId { get; set; } // 因為下拉選單所以給null
        // Migiration預期YachtId = c.Int(nullable: false)

        [Required(ErrorMessage = "評論是必填項")]
        [Display(Name = "評論")]
        public string Comment { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "您必須接受隱私政策")]
        [Display(Name = "接受隱私政策")]  
        public bool AcceptedPrivacy { get; set; }

        [Display(Name = "隱私版本編號")]
        [Range(1, int.MaxValue, ErrorMessage = "請選擇隱私政策版本")]
        public int PrivacyVersionId { get; set; }

        //要在Migration中設定預設值，SubmittedAt = c.DateTime(nullable: false, defaultValueSql: "GETDATE()")
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "提交時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // 告訴 EF 時間統一由資料庫產生
        public DateTime SubmittedAt { get; set; }

        //Navigation property
        [ForeignKey("YachtId")]
        public virtual Yacht.Yacht Yacht { get; set; }

        [ForeignKey("PrivacyVersionId")]
        public virtual PrivacyVersion PrivacyVersion { get; set; }
    }
}