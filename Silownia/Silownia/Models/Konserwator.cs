using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Konserwator : Pracownik
    {
        public virtual ICollection<Konserwacja> Konserwacja { get; set; }
    }
}