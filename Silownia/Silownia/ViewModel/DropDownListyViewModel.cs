using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Silownia.ViewModel
{
    public class DropDownListyViewModel
    {
        [Required(ErrorMessage = "Nie wybrano siłowni")]
        public IList<SelectListItem> SilownieSelectLista { get; set; }
        [Required(ErrorMessage = "Nie wybrano pracownika")] 
        public IList<SelectListItem> PracownikSelectLista { get; set; }
        [Required(ErrorMessage = "Nie wybrano sali")]
        public IList<SelectListItem> SaleSelectLista { get; set; }
        
    }

     
}