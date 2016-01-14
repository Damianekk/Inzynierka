using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Silownia.Models
{
    public class Specjalizacja
    {
        public Specjalizacja()
        {
            Trenerzy = new List<Trener>();
        }

        public long SpecjalizacjaID { get; set; }
        [Required(ErrorMessage = "Podaj nazwę specjalizacji")]
        [Display(Name = "Nazwa specjalizacji")]
        public string Nazwa { get; set; }


        public virtual ICollection<Trener> Trenerzy { get; set; }
    }
}