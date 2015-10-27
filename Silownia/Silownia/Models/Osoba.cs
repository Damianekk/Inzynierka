using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Silownia.Models
{
    [Table("Osoba")]
    public abstract class Osoba 
    {
        [Key]
        [Required]
        public long OsobaID { get; set; }
        [Required]
        public string Imie { get; set; }
        [Required]
        public string Nazwisko { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataUrodzenia { get; set; }
        [Phone]
        [Display(Name = "Nr telefonu")]
        public string NrTelefonu { get; set; }

        [Display(Name = "Imie nazwisko")]
        public string imieNazwisko { get { return Nazwisko + " " + Imie; } }

        public virtual Adres Adres { get; set; }

    }
}