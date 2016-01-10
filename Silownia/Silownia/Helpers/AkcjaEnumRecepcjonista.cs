using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumRecepcjonista
    {
        [Description("Poprawnie dodano recepcjonistę")]
        DodanoRecepcjoniste
       ,
        [Description("Poprawnie usunięto recepcjonistę")]
        UsunietoRecepcjoniste
       , Brak
    }
}