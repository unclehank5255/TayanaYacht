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

        [Required(ErrorMessage = "{0}是必填項")]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [EmailAddress]
        [StringLength(254, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [StringLength(30, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "電話")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "{0}是必填項")]
        [StringLength(60, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "國家")]
        public string Country { get; set; }

        [Display(Name = "游艇編號")]
        [Required(ErrorMessage = "{0}是必填項")]
        public int YachtId { get; set; } // viewModel 設計可null下拉選單這邊不改變資料庫
                                          

        [Display(Name = "游艇名稱快照")]
        [Required]
        [StringLength(2000, ErrorMessage = "{0}不能超過 {1} 個字元")]
        public string YachtNameSnapshot { get; set; } // 用來存當時的YachtName，因為YachtName可能會改變


        [Required(ErrorMessage = "{0}是必填項")]
        [Display(Name = "評論")]
        [StringLength(100)]
        public string Comment { get; set; }

        [Display(Name = "隱私版本編號")]
        [Range(1, int.MaxValue, ErrorMessage = "{0}必填介於 {1} 到 {2} 個字元")]
        public int PrivacyVersionId { get; set; }

        //要在Migration中設定預設值，SubmittedAt = c.DateTime(nullable: false, defaultValueSql: "GETDATE()")
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "提交時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // 告訴 EF 時間統一由資料庫產生
        public DateTime SubmittedAt { get; set; }

        [Display(Name = "處理時間")]

        public DateTime? HandledAt { get; set; }
        //Navigation property
        [ForeignKey("YachtId")]
        public virtual Yacht.Yacht Yacht { get; set; }

        [ForeignKey("PrivacyVersionId")]
        public virtual PrivacyVersion PrivacyVersion { get; set; }
    }
}