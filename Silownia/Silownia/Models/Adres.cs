using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Adres
    {
        public long AdresID { get; set; }
        public string KodPocztowy { get; set; }
        public string Kraj { get; set; }
        public string Miasto { get; set; }
        public string Ulica { get; set; }
        public string NrBudynku { get; set; }
        public string NrLokalu { get; set; }

        [NotMapped]
        public virtual Osoba Osoba { get; set; }
    }
}