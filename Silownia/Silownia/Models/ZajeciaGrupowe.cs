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
            KlientZajeciaGrupowe = new List<KlientZajeciaGrupowe>();
        }
        public string NazwaTreningu { get; set; }

        [MaxLength(300, ErrorMessage = " Opis przekracza dozwoloną ilość słów!")]
        [Display(Name = "Opis")]
        public string OpisTreningu { get; set; }

        [Required]
        public virtual Instruktor Instruktor { get; set; }
        public virtual ICollection<KlientZajeciaGrupowe> KlientZajeciaGrupowe { get; set; }
        [Required]
        public virtual Sala Sala { get; set; }
    }
}