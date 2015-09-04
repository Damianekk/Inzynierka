using System.Collections.Generic;

namespace Silownia.Models
{
    public class Masazystka : Pracownik
    {
        public virtual ICollection<Masaz> Masaz { get; set; }

    }
}