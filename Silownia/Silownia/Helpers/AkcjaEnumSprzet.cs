using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumSprzet
    {
        [Description("Poprawnie dodano sprzęt")]
        DodanoSprzet
       ,
        [Description("Poprawnie usunięto sprzęt")]
        UsunietoSprzet
       , Brak
    }
}