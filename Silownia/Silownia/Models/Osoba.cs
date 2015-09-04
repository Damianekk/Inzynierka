using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public abstract class Osoba 
    {
        public long OsobaID { get; set; }

        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }
        public Adres AdresID { get; set; }

        [Required]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataUrodzenia { get; set; }

        


        [Display(Name = "Imie nazwisko")]
        public string imieNazwisko { get { return Nazwisko + " " + Imie; } }

        public virtual Adres Adres { get; set; }

    }
}