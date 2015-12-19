using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    [Table("Trener")]
    public class Trener : Pracownik
    {
        public long SpecjalizacjaID { get; set; }
   
        public virtual Specjalizacja Specjalizacja { get; set; }
    }
}