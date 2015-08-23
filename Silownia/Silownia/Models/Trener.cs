using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Trener : Pracownik
    {
        public long SpecjalizacjaID { get; set; }

        public virtual Specjalizacja Specjalizacja { get; set; }
    }
}