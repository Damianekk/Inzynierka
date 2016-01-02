using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [Display(Name ="Stawka godzinowa")]
        public int StawkaGodzinowa { get; set; }

        public virtual IList<Masaz> Masaze { get; set; }
        
    }
}