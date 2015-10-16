using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Silownia.Models
{
    public class Silownia 
    {
        public long SilowniaID { get; set; }
        public string Nazwa { get; set; }
        [Display(Name = "Godziny otwarcia")]
        public string GodzinaOtwarcia { get; set; }
        [Display(Name = "Godziny zamknięcia")]
        public string GodzinaZamkniecia { get; set; }
        public string Powierzchnia { get; set; }
        [Display(Name = "Nr kontaktowy")]
        public long NrTelefonu { get; set; }

        
        public virtual Adres Adres { get; set; }
        public virtual ICollection<Umowa> Umowa { get; set; }
    }
}