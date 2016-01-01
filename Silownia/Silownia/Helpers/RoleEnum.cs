using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;


namespace Silownia
{
    public enum RoleEnum
    {
            [Description("Administrator")]
            Administrator,
            [Description("Trener")]
            Trener,
            [Description("Recepcjonista")]
            Recepcjonista,
            [Description("Klient")]
            Klient, 
    }
}

