using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumMasaz
    {
        [Description("Poprawnie dodano masaż")]
        DodanoMasaz
       ,
        [Description("Poprawnie usunięto masaż")]
        UsunietoMasaz
       , Brak
    }
}
