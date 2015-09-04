using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class TreningPersonalny
    {
        public long TreningPersonalnyID { get; set; }

        [Required]
        [Display(Name = "Data treningu")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataTreningu { get; set; }
        public int CzasTrwania { get; set; }
        [Required] 
        public virtual Klient Klient { get; set; }
        [Required] 
        public virtual Trener Trener { get; set; }
    }
}