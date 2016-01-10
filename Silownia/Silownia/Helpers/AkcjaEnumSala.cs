using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia.Helpers
{
    public enum AkcjaEnumSala
    {
       [Description("Poprawnie dodano sale do siłowni!")]
        DodanoSale
       ,
        [Description("Poprawnie usunięto sale z siłowni!")]
        UsunietoSale
       , Brak
    }
}