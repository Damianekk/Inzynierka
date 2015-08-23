using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Umowa
    {
        public long UmowaID { get; set; }
        [Display(Name = "Siłownia")]
        public long SilowniaID { get; set; }


        [Required]
        [Display(Name = "Data podpisania umowy")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataPodpisania { get; set; }

        [Required]
        [Display(Name = "Data zakończenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataZakonczenia { get; set; }

        public string Cena { get; set; }
        [Required]
        public virtual Klient Klient { get; set; }
        [Required]
        public virtual Silownia Silownia { get; set; }
    }
}