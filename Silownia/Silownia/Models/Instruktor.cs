using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Instruktor : Pracownik
    {
        public Instruktor()
        {
            ZajeciaGrup = new List<ZajeciaGrupowe>();
        }    
        public virtual ICollection<ZajeciaGrupowe> ZajeciaGrup { get; set; }
    }
}