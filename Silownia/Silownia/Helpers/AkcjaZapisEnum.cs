using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;


namespace Silownia
{
    public enum AkcjaZapisEnum
    {
       [Description("Udało Ci się zapisać na zajęcia.")]
       DodanoZapis
      ,[Description("Wypisałeś się z zajęć.")]
       UsunietoZapis
       ,[Description("Nie jesteś zapisany na te zajęcia. Nie możesz się z nich wypisać.")]
       NieDaSieZapis
        ,[Description("Już jesteś zapisany na te zajęcia. Nie możesz się na nie znów zapisać.")]
       JuzZapis
      ,Brak
    }
}

