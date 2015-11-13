using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;


namespace Silownia
{
    public enum AkcjaEnum
    {
       [Description("Poprawnie dodano klienta")]
       DodanoKlienta
      ,[Description("Poprawnie usunięto klienta")]
       UsunietoKlienta
      ,Brak
    }
}

