using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TayanaYacht.Models.Dealer
{
    public class Dealer
    {
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "國家編號")]
        [Range(1, int.MaxValue, ErrorMessage = "請選擇國家 ID")]
        public int CountryId { get; set; }

        [StringLength(255, ErrorMessage = "經銷商封面路徑不能超過 255 個字元")]
        [Display(Name = "經銷商封面")]
        public string DealerCoverPath { get; set; }

        [StringLength(100, ErrorMessage = "城市名稱不能超過 100 個字元")]
        [Display(Name = "經銷商名稱")]
        [Required(ErrorMessage = "經銷商名稱是必填項")]
        public string Name { get; set; }

        [Required(ErrorMessage = "城市名稱是必填項")]
        [StringLength(60, ErrorMessage = "城市名稱不能超過 60 個字元")]
        [Display(Name = "城市")] 
        public string City { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "內容是必填項")]
        [Display(Name = "內容")]
        public string ContentHtml { get; set; }

        [Required]
        [Display(Name = "顯示順序")]
        public int DisplayOrder { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}