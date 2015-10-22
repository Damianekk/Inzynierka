using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    [Table("Pracownik")]
    public abstract class Pracownik : Osoba
    {
        [Key, Required]
        public int Pesel { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zatrudnienia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataZatrudnienia { get; set; }
        
        public decimal Pensja { get; set; }

    }
}