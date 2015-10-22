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
        
        public virtual Trener Trener { get; set; }
    }
}
   