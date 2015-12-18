using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class KomentarzOPracowniku
    {
        [Key, Required]
        public long KomentarzID { get; set; }

        [Display(Name = "Komentarz")]
        [MaxLength(250, ErrorMessage = "Wpisany komentarz jest zbyt długi! :(")]
        public string Komentarz { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Data_wystawienia { get; set; }


        [Required(ErrorMessage = "Wybierz ocenę")]
        public int ocena { get; set; }

        public int? KlientID { get; set; }
        public int? PracownikID { get; set; }


        public virtual Klient Klient { get; set; }
        public virtual Pracownik Pracownik { get; set; }
    }
}