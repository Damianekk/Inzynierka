using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Silownia.Models
{
    [Table("Uzytkownicy")]
    public class Uzytkownik
    {
        public Uzytkownik() { }

        [Key]
        public long UzytkownikID { get; set; }

        [Required]
        public long IDOsoby { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; }

        [Required]
        public string Rola { get; set; }
    }

}