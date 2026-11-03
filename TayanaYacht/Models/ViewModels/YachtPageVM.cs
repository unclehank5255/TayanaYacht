using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TayanaYacht.Models.Yacht;

namespace TayanaYacht.Models.ViewModels
{
    public class YachtPageVM
    {
        public List<Yacht.Yacht> Yachts { get; set; }
        public Yacht.Yacht CurrentYacht { get; set; }
        public List<Yacht.YachtPicture> GalleryPictures { get; set; }
        public List<Yacht.YachtPicture> LayoutPictures { get; set; }

        public YachtTab ActiveTab { get; set; }
    }


    public enum YachtTab
    {
        Overview,
        Layout,
        Specification
    }
}