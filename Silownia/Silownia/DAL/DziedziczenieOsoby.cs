using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Silownia.DAL
{
    public class DziedziczenieOsoby : EntityTypeConfiguration<Osoba>
    {
        public DziedziczenieOsoby()
        {
            this.Map<Klient>(x => x.ToTable("Klient"))
                .Map<Pracownik>(x => x.ToTable("Pracownik"));
        }

    }
}