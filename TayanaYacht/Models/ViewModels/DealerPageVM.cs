using System.Collections.Generic;
using TayanaYacht.Models.Dealer;

namespace TayanaYacht.Models.ViewModels
{
    public class DealerPageVM
    {
        public List<Country> Countries { get; set; }
        public Country CurrentCountry { get; set; }
        public List<TayanaYacht.Models.Dealer.Dealer> Dealers { get; set; }
        public string DealerBannerPath { get; set; }
    }
}