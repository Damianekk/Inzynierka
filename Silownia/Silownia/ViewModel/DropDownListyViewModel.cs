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
        public IList<SelectListItem> MasazystaSelectLista { get; set; }
        public IList<SelectListItem> InstruktorSelectLista { get; set; }
        public IList<SelectListItem> TrenerSelectLista { get; set; }
        public IList<SelectListItem> KonserwatorSelectLista { get; set; }
        public IList<SelectListItem> RecepcjonistaSelectLista { get; set; }
        public IList<SelectListItem> SaleSelectLista { get; set; }
       
    }
}