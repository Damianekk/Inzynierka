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
        [Display(Name = "Nr sali")]
        public int Numer_sali { get; set; }
        [Required]
        public string Rodzaj { get; set; }
        [Required]
        public string Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string Opis { get; set; }
        [Required]
        [Display(Name = "Pojemność sali")]
        public int LiczbaOsob { get; set; }

        public virtual ICollection<Sprzet> Sprzety { get; set; }
        public virtual ICollection<ZajeciaGrupowe> ZajeciaGrup { get; set; }
        public virtual ICollection<Masaz> Masaze { get; set; }

        public long SilowniaID { get; set; }
        public virtual Silownia Silownia { get; set; }

    }
}