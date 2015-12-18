using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("Recepcjonista")]
    public class Recepcjonista : Pracownik
    {
        public Recepcjonista()
        {
            Umowy = new List<Umowa>();
        }
        public virtual ICollection<Umowa> Umowy { get; set; }
    }
}