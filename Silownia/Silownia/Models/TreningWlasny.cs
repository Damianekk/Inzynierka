using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("TreningWlasny")]
    public class TreningWlasny : Trening
    {
        public virtual Klient Klient { get; set; }
    }
}