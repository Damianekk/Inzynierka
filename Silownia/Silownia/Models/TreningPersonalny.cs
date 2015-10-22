using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("TreningOersonalny")]
    public class TreningPersonalny : Trening
    {
        public long TreningPersonalnyID { get; set; }

       [Required] 
        public virtual Klient Klient { get; set; }
        [Required] 
        public virtual Trener Trener { get; set; }
    }
}
   