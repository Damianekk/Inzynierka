using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class TreningFitness : Trening
    {
        public long TreningFitnessID { get; set; }
        public string NazwaTreningu { get; set; }
        [MaxLength(300, ErrorMessage=" Opis przekracza dozwoloną ilość słów!")]
        [Display(Name = "Opis")]
        public string OpisTreningu { get; set; }

        public int? KlientID { get; set; }
        public virtual ICollection<Klient> Klient { get; set; }
        [Required]
        public virtual Trener Trener { get; set; }
        public virtual Sala Sala { get; set; }
    }
}