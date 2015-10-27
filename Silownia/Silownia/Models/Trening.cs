﻿using System;
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
        [Display(Name = "Data treningu")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Data_treningu { get; set; }
        [Display(Name = "Godzina rozpoczęcia")]
        public DateTime Godzina_rozpoczecia { get; set; }
         [Display(Name = "Godzina zakończenia")]
        public DateTime Godzina_zakonczenia { get; set; }

         public virtual ICollection<KlientZajeciaGrupowe> KlieniTreningi { get; set; }
         
    }
}