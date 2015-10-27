using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("ZajeciaGrupowe")]
    public class ZajeciaGrupowe : Trening
    {
        public ZajeciaGrupowe()
        {
            Klienci = new List<Klient>();
        }
        public string NazwaTreningu { get; set; }
        [MaxLength(300, ErrorMessage = " Opis przekracza dozwoloną ilość słów!")]
        [Display(Name = "Opis")]
        public string OpisTreningu { get; set; }
        [Required]
        public virtual Trener Trener { get; set; }
        public virtual ICollection<Klient> Klienci { get; set; }
        public virtual Sala Sala { get; set; }
    }
}