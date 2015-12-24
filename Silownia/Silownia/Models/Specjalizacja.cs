using System.ComponentModel.DataAnnotations;
namespace Silownia.Models
{
    public class Specjalizacja
    {
        public long SpecjalizacjaID { get; set; }
        [Required]
        [Display(Name = "Nazwa specjalizacji")]
        public string Nazwa { get; set; }

    }
}