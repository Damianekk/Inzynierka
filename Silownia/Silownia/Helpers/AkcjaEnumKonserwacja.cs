using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Silownia
{
    public enum AkcjaEnumKonserwacja
    {
        [Description("Poprawnie dodano konserwację")]
        DodanoKonserwacje
       ,
        [Description("Poprawnie usunięto konserwację")]
        UsunietoKonserwacje
       ,
        [Description("Przyjęto konserwację")]
        PrzyjetoKonserwacje
       ,
        [Description("Zamknięto konserwację")]
        ZamknietoKonserwacje
       ,
        [Description("Pomyslnie wprowadzono zmiany w konserwacji")]
        ZmienionoKonserwacje
        ,
        Brak
    }
}
