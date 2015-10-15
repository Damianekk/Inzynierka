using System.Collections.Generic;
namespace Silownia.Models
{
    public class Silownia 
    {
        public long SilowniaID { get; set; }
        public string Nazwa { get; set; }
        public string GodzinaOtwarcia { get; set; }
        public string GodzinaZamkniecia { get; set; }
        public string Powierzchnia { get; set; }
        public long NrTelefonu { get; set; }

        
        public virtual Adres Adres { get; set; }
        public virtual ICollection<Umowa> Umowa { get; set; }
    }
}