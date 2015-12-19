using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Masaz
    {
        public long MasazID { get; set; }
        [Display(Name = "Typ masażu")]
        public string Typ { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Początek masażu")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Poczatek { get; set; }
        [Display(Name = "Koniec masażu")]
        public DateTime Koniec { get; set; }
        public TimeSpan CzasTrwania { get { return this.Koniec - this.Poczatek; } set { CzasTrwania = value; } }
        [Required]
        public virtual Klient Klient { get; set; }
        [Required]
        [Display(Name = "Masażysta")]
        public virtual Masazysta Masazysta { get; set; }
        public virtual Sala Sala { get; set; }


        public Masaz(DateTime poczatek, DateTime koniec, TimeSpan czasTrwania, Klient klient, Masazysta masazysta, Sala sala)
        {
            this.Poczatek = poczatek;
            this.Koniec = koniec;
            this.CzasTrwania = czasTrwania;
            this.Klient = klient;
            this.Masazysta = masazysta;
            this.Sala = sala;
        }
    }
}