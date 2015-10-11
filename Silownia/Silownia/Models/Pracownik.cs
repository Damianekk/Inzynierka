using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Pracownik : Osoba
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zatrudnienia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataZatrudnienia { get; set; }

        public string Pensja { get; set; }

    }
}