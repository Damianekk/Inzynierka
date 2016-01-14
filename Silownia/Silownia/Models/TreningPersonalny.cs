using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class TreningPersonalny : Trening
    {
        [Required(ErrorMessage = "Wybierz trenera z listy")]
        public long TrenerID { get; set; }

        [Display(Name = "Całkowity koszt treningu")]
        public int kosztTreningu { get; set; }

        public virtual Klient Klient { get; set; }
        public virtual Trener Trener { get; set; }

    }
}