using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("KlientZajeciaGrupowe")]
    public class KlientZajeciaGrupowe
    {
        [Key, Column(Order = 0)]
        public long OsobaID { get; set; }
        [Key, Column(Order = 1)]
        public int TreningID { get; set; }

        public virtual Klient Klient { get; set; }
        public virtual ZajeciaGrupowe ZajeciaGrupowe { get; set; }

    }
} 