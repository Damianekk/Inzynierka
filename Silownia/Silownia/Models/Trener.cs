using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    [Table("Trener")]
    public class Trener : Pracownik
    {
        public Trener()
        {
            TreningiPersonalne = new List<TreningPersonalny>();
        }

        [Required]
        [Display(Name = "Stawka za godzinę pracy")]
        public int StawkaGodzinowa { get; set; }

        public long SpecjalizacjaID { get; set; }

        public virtual Specjalizacja Specjalizacja { get; set; }

        public virtual ICollection<TreningPersonalny> TreningiPersonalne { get; set; }


    }
}