using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Adres
    {
        public long AdresID { get; set; }
        [Display(Name = "Kod pocztowy")]
        public string KodPocztowy { get; set; }
        public string Kraj { get; set; }
        public string Miasto { get; set; }
        public string Ulica { get; set; }
        [Display(Name = "Nr budynku")]
        public string NrBudynku { get; set; }
        [Display(Name = "Nr lokalu")]
        public string NrLokalu { get; set; }

        [NotMapped]
        public virtual Osoba Osoba { get; set; }
    }
}