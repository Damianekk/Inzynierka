using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("Instruktor")]
    public class Instruktor : Pracownik
    {
        public Instruktor()
        {
            ZajeciaGrup = new List<ZajeciaGrupowe>();
        }    
        public virtual ICollection<ZajeciaGrupowe> ZajeciaGrup { get; set; }
    }
}