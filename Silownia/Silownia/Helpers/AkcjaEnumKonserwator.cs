using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumKonserwator
    {
        [Description("Poprawnie dodano konserwatora")]
        DodanoKonserwatora
       ,
        [Description("Poprawnie usunięto konserwatora")]
        UsunietoKonserwatora
       , Brak
    }
}