using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumUmowa
    {
        [Description("Poprawnie dodano umowę")]
        DodanoUmowe
       ,
        [Description("Poprawnie usunięto umowę")]
        UsunietoUmowe
       , Brak
    }
}