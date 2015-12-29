using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumTrening
    {
        [Description("Poprawnie dodano trening")]
        DodanoTrening
       ,
        [Description("Poprawnie usunięto trening")]
        UsunietoTrening
       , Brak
    }
}
