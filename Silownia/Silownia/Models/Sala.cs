using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Sala
    {
        public Sala()
        {
            ZajeciaGrup = new List<ZajeciaGrupowe>();
            Sprzety = new List<Sprzet>();
            Masaze = new List<Masaz>();
        }


        [Key, Required]
       
        [Display(Name="Nr sali")]
        public int Numer_sali { get; set; }
        public string ImageFile { get; set; }
        public string Rodzaj { get; set; }
        public string Status { get; set; }
        public string Opis { get; set; }
        public virtual ICollection<Sprzet> Sprzety { get; set; }
        public virtual ICollection<ZajeciaGrupowe> ZajeciaGrup { get; set; }
        public virtual ICollection<Masaz> Masaze { get; set; }
        public virtual Silownia Silownia { get; set; }

    }
}