using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silownia
{
    public enum AkcjaEnumMasazysta
    {
        [Description("Poprawnie dodano masażystę")]
        DodanoMasazyste
       ,
        [Description("Poprawnie usunięto masażystę")]
        UsunietoMasazyste
       , Brak
    }
}
