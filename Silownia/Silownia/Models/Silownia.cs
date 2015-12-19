using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string Nazwa { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Godziny otwarcia")]
        public string GodzinaOtwarcia { get; set; }
        [DataType(DataType.Time)]
        [Display(Name = "Godziny zamknięcia")]
        public string GodzinaZamkniecia { get; set; }
        public string Powierzchnia { get; set; }
        [Display(Name = "Nr kontaktowy")]
        [DataType(DataType.PhoneNumber)]
        public long NrTelefonu { get; set; }
        public double Dlugosc { get; set; }
        public double Szerokosc { get; set; }

        public virtual Adres Adres { get; set; }
        public virtual ICollection<Umowa> Umowy { get; set; }
        public virtual ICollection<Pracownik> Pracownicy { get; set; }
        public virtual ICollection<Sala> Sale { get; set; }
    }
}