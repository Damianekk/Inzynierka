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
        [Display(Name = "Pełna data masażu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DataMasazu { get; set; }

        [Display(Name = "Czas trwania masażu")]
        public int CzasTrwania { get; set; }


        [Display(Name = "Data końca masażu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime DataMasazuKoniec { get; set; }


        [Display(Name = "Całkowity koszt masażu")]
        public int kosztMasazu { get; set; }


        public virtual Klient Klient { get; set; }
        public virtual Masazysta Masazysta { get; set; }
    }
}