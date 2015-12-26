using System;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Umowa
    {
        [Key, Required]
        public long UmowaID { get; set; }

        [Required(ErrorMessage = "Wybierz siłownię z listy")]
        public long SilowniaID { get; set; }

        [Required(ErrorMessage = "Wybierz recepcjonistę z listy")]
        public long RecepcjonistaID { get; set; }

        [Required(ErrorMessage = "Podaj datę rozpoczęcia")]
        [DataType(DataType.Date)]
        [Display(Name = "Data podpisania")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataPodpisania { get; set; }

        [Required(ErrorMessage = "Podaj datę zakończenia")]
        [DataType(DataType.Date)]
        [Display(Name = "Data zakończenia")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataZakonczenia { get; set; }

        [RegularExpression("\\d{1,3}[,\\.]?(\\d{1,2})?", ErrorMessage = "Wprowadź poprawną kwotę")]
        [Required(ErrorMessage = "Uzupełnij koszt karnetu/mc")]
        [Display(Name = "Opłata /msc")]
        public string Cena { get; set; }


        public virtual Klient Klient { get; set; }
        public virtual Recepcjonista Recepcjonista { get; set; }
        public virtual Silownia Silownia { get; set; }
    }
}