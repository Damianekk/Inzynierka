using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumTrener
    {
        [Description("Poprawnie dodano trenera")]
        DodanoTrenera
       ,
        [Description("Poprawnie usunięto trenera")]
        UsunietoTrenera
       , Brak
    }
}