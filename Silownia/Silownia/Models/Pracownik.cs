using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Pracownik : Osoba
    {
        [Required]
        [Display(Name = "Data zatrudnienia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataZatrudnienia { get; set; }

        public string Pensja { get; set; }

    }
}