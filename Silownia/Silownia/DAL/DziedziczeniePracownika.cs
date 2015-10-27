using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Silownia.DAL
{
    public class DziedziczeniePracownika : EntityTypeConfiguration<Pracownik>
    {
        public DziedziczeniePracownika()
        {
            this.Map<Trener>(x => x.ToTable("Trener"))
                .Map<Konserwator>(x => x.ToTable("Konserwator"))
                .Map<Masazysta>(x => x.ToTable("Masazysta"))
                .Map<Instruktor>(x => x.ToTable("Instruktor"))
                .Map<Recepcjonista>(x => x.ToTable("Recepcjonista"));
        }
    }
}