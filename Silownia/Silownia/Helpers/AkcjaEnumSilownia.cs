using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumSilownia
    {
        [Description("Poprawnie dodano siłownię")]
        DodanoSilownie
       ,
        [Description("Poprawnie usunięto siłownię")]
        UsunietoSilownie
       , Brak
    }
}