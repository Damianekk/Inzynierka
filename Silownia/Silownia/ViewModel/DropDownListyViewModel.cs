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
        public IList<SelectListItem> PracownikSelectLista { get; set; }
      
        public IList<SelectListItem> SaleSelectLista { get; set; }
       
    }
}