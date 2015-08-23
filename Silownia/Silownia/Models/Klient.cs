using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Klient : Osoba
    {
        [EmailAddress]
        public string Mail { get; set; }
        [Phone]
        [Display(Name = "Nr telefonu")]
        public string NrTelefonu { get; set; }
        
        public virtual ICollection<TreningPersonalny> TreningPersonalny { get; set; }
        public virtual ICollection<Umowa> Umowa { get; set; }
        public virtual ICollection<Masaz> Masaz { get; set; }



    }
}