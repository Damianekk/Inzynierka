using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silownia.ViewModel
{
    public class DropDownListyViewModel
    {
        public IList<SelectListItem> SilownieSelectLista { get; set; }
        public IList<SelectListItem> TrenerzySelectLista { get; set; }
        public IList<SelectListItem> InstruktorzySelectLista { get; set; }
        public IList<SelectListItem> SaleSelectLista { get; set; }
        public IList<SelectListItem> KonserwatorzySelectLista { get; set; }
    }
}