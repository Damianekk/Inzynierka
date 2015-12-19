using Silownia.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Wiadomosc
    {
        [Key]
        public long WiadomoscID { get; set; }
        public string Tresc { get; set; }
        public virtual Osoba OsobaWysylajaca { get; set; } 
        public virtual Osoba OsobaOdbierajaca { get; set; }
        public StatusWiadomosciEnum Status { get;  set;}
        public DateTime? Wyslano { get; set; }
        public DateTime?Odebrano { get; set; }


    }
}