using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Silownia.Dal
{
    public class MapowanieDziedziczenia : SilowniaContext
    {
        public DbSet<Osoba> Osoby { get; set; }
        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<Trening> Treningi { get; set; }

    }
}