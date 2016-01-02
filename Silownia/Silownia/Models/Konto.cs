using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    public class Konto
    {
        [Key]
        public long KontoID { get; set; }
        public string Login { get; set; }
        public string Haslo { get; set; }
        public DateTime DataUtworzenia { get; set; }

    }
}