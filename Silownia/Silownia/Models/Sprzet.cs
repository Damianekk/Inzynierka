using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Sprzet
    {
        public Sprzet()
        {
            Konserwacje = new List<Konserwacja>();
        }

        [Key, Required]
        public long SprzetID { get; set; }
        [Required]
        public string Nazwa { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Data_zakupu { get; set; }
        [Display(Name="Cena zakupu")]
        public decimal Cena_zakupu { get; set; }

        public virtual ICollection<Konserwacja> Konserwacje { get; set; }
        public virtual Sala Sala { get; set; }

    }
}