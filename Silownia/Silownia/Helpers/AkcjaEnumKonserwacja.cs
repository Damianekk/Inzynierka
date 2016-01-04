using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumKonserwacja
    {
        [Description("Poprawnie dodano konserwację")]
        DodanoKonserwacje
       ,
        [Description("Poprawnie usunięto konserwację")]
        UsunietoKonserwacje
       , Brak
    }
}
