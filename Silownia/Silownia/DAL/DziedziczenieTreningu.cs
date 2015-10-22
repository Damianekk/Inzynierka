using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Silownia.DAL
{
    public class DziedziczenieTreningu : EntityTypeConfiguration<Trening>
    {
        public DziedziczenieTreningu()
        {
            this.Map<TreningPersonalny>(x => x.ToTable("TreningPersonalny"))
                .Map<TreningWlasny>(x => x.ToTable("TreningWlasny"))
                .Map<TreningFitness>(x => x.ToTable("TreningFitness"));
        }
    }
}