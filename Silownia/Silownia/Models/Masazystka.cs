using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Masazystka : Pracownik
    {
        public virtual ICollection<Masaz> Masaz { get; set; }

    }
}