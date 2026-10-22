using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TayanaYacht.Models.News;

namespace TayanaYacht.Models.Dealer
{
    public class Country
    {
        public Country()
        {
            Dealers = new HashSet<Dealer>();
        }
        [Key]
        [Display(Name = "編號")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "國家不能超過 60 個字元")]
        [Display(Name = "國家")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "顯示順序")]
        public int DisplayOrder { get; set; }

        public virtual ICollection<Dealer> Dealers { get; set; }
    }
}