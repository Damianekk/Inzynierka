using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Umowa
    {
        public long UmowaID { get; set; }
        
        [Required]
        public long SilowniaID { get; set; }
        [Required]
        public long KlientID { get; set; }
        [Required]
        public long RecepcjonistaID { get; set; }
 

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
        public virtual Klient Klient { get; set; }
        public virtual Recepcjonista Recepcjonista { get; set; }
        public virtual Silownia Silownia { get; set; }
    }
}