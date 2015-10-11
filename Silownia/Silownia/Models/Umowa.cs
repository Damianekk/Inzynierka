using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Umowa
    {
        public long UmowaID { get; set; }
        [Display(Name = "Siłownia")]
        public long SilowniaID { get; set; }

 

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data podpisania umowy")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataPodpisania { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zakończenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DataZakonczenia { get; set; }

        public string Cena { get; set; }
        [Required]
        public virtual Klient Klient { get; set; }
        [Required]
        public virtual Recepcjonista Recepcjonista { get; set; }
        [Required]
        public virtual Silownia Silownia { get; set; }
    }
}