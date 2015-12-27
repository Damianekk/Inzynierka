using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Masaz
    {
        [Required(ErrorMessage = "Wybierz masażystę z listy")]
        public long MasazystaID { get; set; }

        public long MasazID { get; set; }

        [Required(ErrorMessage = "Podaj datę masażu")]
        [DataType(DataType.Date)]
        [Display(Name = "Data masażu")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataMasazu { get; set; }

        [Required(ErrorMessage = "Podaj godzinę masażu")]
        [DataType(DataType.Time)]
        [Display(Name = "Godzina masażu")]
        public DateTime GodzinaMasazuOd { get; set; }

        [Display(Name = "Czas trwania")]
        public int CzasTrwania { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany!")]
        public virtual Klient Klient { get; set; }
        [Required(ErrorMessage = "Masażysta jest wymagany!")]
        public virtual Masazysta Masazysta { get; set; }
    }
}