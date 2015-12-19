using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
  
    public abstract class Trening
    {
        [Key,Required]
        public int TreningID { get; set; }
        [Display(Name = "Tytuł")]
        public string Tytul { get; set; }
        [Required]
        [Display(Name = "Początek")]
        public DateTime Poczatek { get; set; }
         [Display(Name = "Koniec")]
        public DateTime Koniec { get; set; }
         
        [MaxLength(300, ErrorMessage = " Opis przekracza dozwoloną ilość słów!")]
         [Display(Name = "Opis")]
         public string OpisTreningu { get; set; }





    }
}