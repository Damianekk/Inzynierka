using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("TreningFitness")]
    public class TreningFitness : Trening
    {
        public TreningFitness()
        {
            Klient = new Klient();
        }
        public string NazwaTreningu { get; set; }
        [MaxLength(300, ErrorMessage = " Opis przekracza dozwoloną ilość słów!")]
        [Display(Name = "Opis")]
        public string OpisTreningu { get; set; }
        [Required]
        public virtual Trener Trener { get; set; }
        public virtual Sala Sala { get; set; }
    }
}