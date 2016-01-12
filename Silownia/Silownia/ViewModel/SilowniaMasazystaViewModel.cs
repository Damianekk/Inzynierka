using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silownia.ViewModel
{
    public class SilowniaMasazystaViewModel
    {
        public IList<SelectListItem> SilownieSelectLista { get; set; }
        public IList<SelectListItem> MasazysciSelectLista { get; set; }
    }
}