using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silownia.ViewModel
{
    public class MasazystaWSilowniViewModel
    {
        public IList<SelectListItem> MasazystaSelectLista { get; set; }
        public IList<SelectListItem> SilownieSelectLista { get; set; }
    }
}