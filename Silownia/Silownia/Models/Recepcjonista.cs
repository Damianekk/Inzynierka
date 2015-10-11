using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Recepcjonista : Pracownik
    {
        public virtual ICollection<Umowa> Umowa { get; set; }
    }
}