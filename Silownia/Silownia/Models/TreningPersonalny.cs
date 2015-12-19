using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class TreningPersonalny : Trening
    {
        public virtual Klient Klient { get; set; }
        public virtual Trener Trener { get; set; }

        public TreningPersonalny()
        {

        }

        //public virtual IQuerable<Trening> GetAll()
        //{

        //}
    }
}