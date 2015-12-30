using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("Trening")]
    public abstract class Trening
    {
        [Key, Required]
        public int TreningID { get; set; }

        [Required(ErrorMessage = "Podaj datę treningu")]
        [Display(Name = "Początek treningu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime TreningStart { get; set; }


        [Required(ErrorMessage = "Podaj datę treningu")]
        [Display(Name = "Koniec treningu")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime TreningKoniec { get; set; }

        [Display(Name = "Czas trwania treningu")]
        public int CzasTrwania { get; set; }
    }
}