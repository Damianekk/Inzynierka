using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    [Table("Klient")]
    public class Klient : Osoba
    {

        public Klient()
        {
            Umowy = new List<Umowa>();
            Masaze = new List<Masaz>();
            KlienciTreningiGrupowe = new List<KlientZajeciaGrupowe>();
            KomentarzeOPracowniku = new List<KomentarzOPracowniku>();
            TreningiPersonalne = new List<TreningPersonalny>();
        }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        public virtual ICollection<Umowa> Umowy { get; set; }
        public virtual ICollection<Masaz> Masaze { get; set; }
        public virtual ICollection<TreningPersonalny> TreningiPersonalne { get; set; }
        public virtual ICollection<KlientZajeciaGrupowe> KlienciTreningiGrupowe { get; set; }
        public virtual ICollection<KomentarzOPracowniku> KomentarzeOPracowniku { get; set; }
    }
}