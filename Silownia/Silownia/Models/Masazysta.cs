using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Masazysta : Pracownik
    {
       public Masazysta()
        {
            Masaze = new List<Masaz>();
        }

        public virtual IList<Masaz> Masaze { get; set; }
        
    }
}