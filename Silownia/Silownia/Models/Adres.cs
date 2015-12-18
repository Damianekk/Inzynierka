using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    public class Adres
    {
        public long AdresID { get; set; }
        [DataType(DataType.PostalCode)]
        [Display(Name = "Kod pocztowy")]
        public string KodPocztowy { get; set; }
        [Required(ErrorMessage = "Podaj kraj")]
        public string Kraj { get; set; }
        [Required(ErrorMessage = "Podaj miasto")]
        public string Miasto { get; set; }
        [Required(ErrorMessage = "Podaj ulicę")]
        public string Ulica { get; set; }
        [Required(ErrorMessage = "Podaj numer budynku")]
        [Display(Name = "Numer budynku")]
        public string NrBudynku { get; set; }

        [Display(Name = "Numer lokalu")]
        public string NrLokalu { get; set; }

        [NotMapped]
        public virtual Osoba Osoba { get; set; }

        [NotMapped]
        public virtual Silownia Silownia { get; set; }
    }
}