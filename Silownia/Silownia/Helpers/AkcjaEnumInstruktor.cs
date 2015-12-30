using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumInstruktor
    {
        [Description("Poprawnie dodano instruktora")]
        DodanoInstruktora
       ,
        [Description("Poprawnie usunięto instruktora")]
        UsunietoInstruktora
       , Brak
    }
}