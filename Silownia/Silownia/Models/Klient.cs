using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    public class Klient : Osoba
    {

        public Klient()
        {
            Trening = new List<Trening>();
            Masaz = new List<Masaz>();
            Umowa = new List<Umowa>();
            KomentarzOPracowniku = new List<KomentarzOPracowniku>();
        }

        public long KlientID { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        [Phone]
        [Display(Name = "Nr telefonu")]
        public string NrTelefonu { get; set; }

        public virtual ICollection<Umowa> Umowa { get; set; }
        public virtual ICollection<Masaz> Masaz { get; set; }
        public virtual ICollection<Trening> Trening { get; set; }
        public virtual ICollection<KomentarzOPracowniku> KomentarzOPracowniku { get; set; }


    }
}