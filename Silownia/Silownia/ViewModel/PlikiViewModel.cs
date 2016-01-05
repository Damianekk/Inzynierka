using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Silownia.Models;
using System.ComponentModel.DataAnnotations;

namespace Silownia.ViewModel
{
    public class PlikiViewModel
    {
        [Display(Name = "Nr sali")]
        public int Numer_sali { get; set; }

        [Required]
        public string Rodzaj { get; set; }
        public string Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string Opis { get; set; }

        public virtual ICollection<Sprzet> Sprzety { get; set; }
        public virtual ICollection<ZajeciaGrupowe> ZajeciaGrup { get; set; }
        public virtual ICollection<Masaz> Masaze { get; set; }

        public long SilowniaID { get; set; }
        public virtual Models.Silownia Silownia { get; set; }


        public IEnumerable<HttpPostedFileBase> File { get; set; }
        public IEnumerable<Sala> Sale { get; set; }
    }
}