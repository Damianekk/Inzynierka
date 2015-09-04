namespace Silownia.Models
{
    public class Trener : Pracownik
    {
        public long SpecjalizacjaID { get; set; }

        public virtual Specjalizacja Specjalizacja { get; set; }
    }
}