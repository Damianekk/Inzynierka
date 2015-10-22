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
        public virtual ICollection<Konserwacja> Konserwacja { get; set; }
    }
}