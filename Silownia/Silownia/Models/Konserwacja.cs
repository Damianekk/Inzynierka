using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Konserwacja
    {
        [Key, Required]
        public long KonserwacjaID { get; set; }
        public string Opis_usterki { get; set; }
       
        [DisplayFormat(DataFormatString="{0:dd/MM/yyyy hh:mm:ss }")]
        public DateTime Data_zgłoszenia { get; set; }
       
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss }")]
        public DateTime Data_naprawy { get; set; }

        public string Status { get; set; }
        public int? KonserwatorID { get; set; }
        public virtual Konserwator Konserwator { get; set; }
        public virtual Sprzet Sprzet { get; set; }

    }
}