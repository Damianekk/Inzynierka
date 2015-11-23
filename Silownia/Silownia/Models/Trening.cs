using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("Trening")]
    public abstract class Trening
    {
        [Key,Required]
        public int TreningID { get; set; }
        [Required]
        [Display(Name = "Początek")]
        public DateTime Poczatek { get; set; }
         [Display(Name = "Koniec")]
        public DateTime Koniec { get; set; }
    }
}