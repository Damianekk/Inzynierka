using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    public class Silownia 
    {
        public Silownia()
        {
            Umowy = new List<Umowa>();
            Pracownicy = new List<Pracownik>();
            Sale = new List<Sala>();
        }
        [Key, Required]
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public long SilowniaID { get; set; }

        [Display(Name = "Nazwa siłowni")]
        [Required(ErrorMessage = "Podaj nazwę siłowni")]
        public string Nazwa { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Godziny otwarcia")]
        [Required(ErrorMessage = "Podaj godzinę otwarcia")]
        public string GodzinaOtwarcia { get; set; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Podaj godzinę zamknięcia")]
        [Display(Name = "Godziny zamknięcia")]
        public string GodzinaZamkniecia { get; set; }

        public string Powierzchnia { get; set; }

        [Display(Name = "Nr kontaktowy")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Podaj kontaktowy numer telefonu")]
        public long NrTelefonu { get; set; }

        public double Dlugosc { get; set; }
        public double Szerokosc { get; set; }

        [Display(Name = "Dodatkowe informacje")]
        [Required(ErrorMessage = "Podaj dodatkowe info")]
        public string DodatkoweInfo { get; set; }
      
        [NotMapped]
        public string infoDodatkowe
        {
            get
            {
                return !string.IsNullOrEmpty(DodatkoweInfo) ? DodatkoweInfo : "Brak";
            }
            set
            {
                DodatkoweInfo = value;
            }
        }

        public virtual Adres Adres { get; set; }
        public virtual ICollection<Umowa> Umowy { get; set; }
        public virtual ICollection<Pracownik> Pracownicy { get; set; }
        public virtual ICollection<Sala> Sale { get; set; }
    }
}