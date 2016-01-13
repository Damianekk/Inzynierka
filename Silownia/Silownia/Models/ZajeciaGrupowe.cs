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

        [Required(ErrorMessage = "Wybierz instruktora z listy")]
        public long InstruktorID { get; set; }

        [Required(ErrorMessage = "Wybierz salę z listy")]
        public long SalaID { get; set; }

        [MaxLength(15, ErrorMessage = " Opis przekracza dozwoloną ilość znaków (15)")]
        [Display(Name = "Nazwa treningu")]
        public string NazwaTreningu { get; set; }

        [MaxLength(300, ErrorMessage = " Opis przekracza dozwoloną ilość znaków (300)")]
        [Display(Name = "Opis")]
        public string OpisTreningu { get; set; }

        [Display(Name = "Ilość zapisanych osób")]
        public int ZapisaneOsoby { get; set; }

        [Required]
        public virtual Instruktor Instruktor { get; set; }
        public virtual ICollection<KlientZajeciaGrupowe> KlientZajeciaGrupowe { get; set; }

        [Required]
        public virtual Sala Sala { get; set; }
    }
}