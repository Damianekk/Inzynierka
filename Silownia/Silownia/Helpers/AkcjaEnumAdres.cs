using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumAdres
    {
        [Description("Poprawnie dodano adres")]
        DodanoAdres
       ,
        [Description("Poprawnie usunięto adres")]
        UsunietoAdres
       , Brak
    }
}