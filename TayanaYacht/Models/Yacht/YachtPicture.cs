using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // 新增
using System.ComponentModel.DataAnnotations.Schema; // 加上這行
using System.Linq;
using System.Web;

namespace TayanaYacht.Models.Yacht
{
    
    public enum YachtPictureTypeEnum //使用者如果沒選圖片分類，預設為Gallery
    {
        //如果未來新增欄位需要同步維護
        Gallery,
        Layout,
        Dimension
    }
    public class YachtPicture
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
 
        [Display(Name = "Yacht編號")]
        [Range(1, int.MaxValue, ErrorMessage = "請選擇遊艇")]
        public int YachtId { get; set; }

        [Range(0, 2, ErrorMessage = "圖片分類必須在 0 到 2 之間")]
        [Display(Name = "圖片分類")]
        public YachtPictureTypeEnum PictureType { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "圖片路徑不能超過 255 個字元")]
        [Display(Name = "圖片路徑")]
        public string ImagePath { get; set; }

        [Display(Name = "顯示順序")]
        public int DisplayOrder { get; set; }

        // Navigation properties
        [ForeignKey("YachtId")]
        public virtual Yacht Yacht { get; set; }
    }
}