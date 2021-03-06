﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Konserwacja
    {
        [Key, Required]
        public long KonserwacjaID { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Opis usterki")]
        [MaxLength(250, ErrorMessage = "Opis usterki jest zbyt długi! :(")]
        public string Opis_usterki { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zgłoszenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Data_zgłoszenia { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data naprawy")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Data_naprawy { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "Wybierz konserwatora z listy")]
        public int KonserwatorID { get; set; }

        public virtual Konserwator Konserwator { get; set; }
        public virtual Sprzet Sprzet { get; set; }

    }
}