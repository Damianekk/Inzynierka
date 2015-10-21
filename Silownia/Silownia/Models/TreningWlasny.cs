using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class TreningWlasny : Trening
    {
        public long TreningWlasnyID { get; set; }

        [Required]
        public virtual Klient Klient { get; set; }

    }
}