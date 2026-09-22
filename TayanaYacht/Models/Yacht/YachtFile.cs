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
        [StringLength(50, ErrorMessage = "文件名不能超過 50 個字元")]
        [Display(Name = "原始文件名")]
        public string FileOriginalName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "實際文件名不能超過 50 個字元")]
        [Display(Name = "實際文件名")]
        public string StoredFileName { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "文件路徑不能超過 255 個字元")]
        [Display(Name = "文件路徑")]
        public string FilePath { get; set; }

        // Navigation property
        [ForeignKey("YachtId")]
        public virtual Yacht Yacht { get; set; }

    }
}