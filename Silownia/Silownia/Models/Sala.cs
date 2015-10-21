using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Sala
    {
        public Sala()
        {
            TreningFitness = new List<TreningFitness>();
        }


        [Key, Required]
        [Display(Name="Nr sali")]
        public int Numer_sali { get; set; }
        public string Status { get; set; }

        public virtual ICollection<TreningFitness> TreningFitness { get; set; } 
        public virtual Silownia Silownia { get; set; }

    }
}