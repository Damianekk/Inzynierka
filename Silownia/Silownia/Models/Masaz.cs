using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Masaz
    {
        public long MasazID { get; set; }
 

        [Required]
        [Display(Name = "Data masażu")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataMasazu { get; set; }

        public int CzasTrwania { get; set; }
        [Required]
        public virtual Klient Klient { get; set; }
        [Required]
        public virtual Masazystka Masazystka { get; set; }
    }
}