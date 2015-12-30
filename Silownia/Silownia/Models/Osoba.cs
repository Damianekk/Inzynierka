using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Silownia.Models
{
    [Table("Osoba")]
    public abstract class Osoba
    {
        public Osoba()
        {
            Wiadomosci = new List<Wiadomosc>();
        }


        [Key]
        [Required]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public long OsobaID { get; set; }

        [Required]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

        [Required]
        public string Nazwisko { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataUrodzenia { get; set; }

        public byte[] ZdjecieProfilowe { get; set; }

        [Phone]
        [Display(Name = "Numer telefonu")]
        public string NrTelefonu { get; set; }

        [Display(Name = "Imię i nazwisko")]
        public string imieNazwisko { get { return Imie + " " + Nazwisko; } }

        public virtual Adres Adres { get; set; }
        public virtual ICollection<Wiadomosc> Wiadomosci { get; set; }

    }
}