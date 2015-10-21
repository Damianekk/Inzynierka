using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class InstruktorFitness : Pracownik
    {
        public InstruktorFitness()
        {
            TreningFitness = new List<TreningFitness>();
        }    
        public virtual ICollection<TreningFitness> TreningFitness { get; set; }
    }
}