using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("Konserwator")]
    public class Konserwator : Pracownik
    {
        public Konserwator()
        {
            Konserwacje = new List<Konserwacja>();
        }

        public virtual ICollection<Konserwacja> Konserwacje { get; set; }
    }
}