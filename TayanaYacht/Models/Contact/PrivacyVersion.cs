using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TayanaYacht.Models.Contact
{
    public class PrivacyVersion
    {
        // Constructor to initialize the Contacts and ContactMessages collections
        public PrivacyVersion()
        {
            Contacts = new HashSet<Contact>();
            ContactMessages = new HashSet<ContactMessage>();
        }
        //Primary key
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "版本不能超過 50 個字元")]
        [Display(Name = "版本")]
        public string Version { get; set; }

        [Required]
        [Display(Name = "內容")]
        public string ContentHtml { get; set; }

        //要在Migration中設定預設值，CreatedAt = c.DateTime(nullable: false, defaultValueSql: "GETDATE()")
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "創建時間")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // 告訴 EF 時間統一由資料庫產生
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<ContactMessage> ContactMessages { get; set; }
    }
}