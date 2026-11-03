using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TayanaYacht.Models.Yacht
{
    public class YachtFile
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        [Display(Name = "Yacht編號")]
        [Range(1, int.MaxValue, ErrorMessage = "請選擇遊艇")]
        public int YachtId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "文件顯示名")]
        public string FileDisplayName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "文件儲存名")]
        public string StoredFileName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "{0}不能超過 {1} 個字元")]
        [Display(Name = "文件路徑")]
        public string FilePath { get; set; }

        // Navigation property
        [ForeignKey("YachtId")]
        public virtual Yacht Yacht { get; set; }

    }
}