using System.Collections.Generic;

namespace Silownia.Models
{
    public class Masazystka : Pracownik
    {
        public Masazystka()
        {
            Masaz = new List<Masaz>();
        }
        public virtual ICollection<Masaz> Masaz { get; set; }

    }
}