using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Sprzet
    {
        public Sprzet()
        {
            Konserwacje = new List<Konserwacja>();
        }

        [Key, Required]
        public long SprzetID { get; set; }
        [Required(ErrorMessage = "Podaj nazwę")]
        [Display(Name = "Nazwa")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Podaj datę zakupu")]
        [Display(Name = "Data zakupu")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Data_zakupu { get; set; }

        [Required(ErrorMessage = "Podaj cenę zakupu")]
        [Display(Name = "Cena zakupu")]
        public decimal Cena_zakupu { get; set; }
        //public int Cena_zakupu { get; set; }

        //[Required(ErrorMessage = "Podaj ilość sztuk")]
        //[Display(Name = "Ilość sztuk")]
        //public int ilosc_sztuk { get; set; }

        public virtual ICollection<Konserwacja> Konserwacje { get; set; }
        public virtual Sala Sala { get; set; }

    }
}