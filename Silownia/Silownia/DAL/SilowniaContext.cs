using Silownia.Models;
using Silownia.DAL;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Silownia.DAL
{
    public class SilowniaContext : DbContext
    {
        public SilowniaContext()
            : base("SilowniaContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SilowniaContext, Silownia.Migrations.Configuration>("SilowniaContext"));
        }

        public DbSet<Adres> Adresy { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Masaz> Masaze { get; set; }
        public DbSet<Masazysta> Masazysci { get; set; }
        public DbSet<Models.Silownia> Silownie { get; set; }
        public DbSet<Specjalizacja> Specjalizacje { get; set; }
        public DbSet<Trener> Trenerzy { get; set; }
        public DbSet<InstruktorFitness> InstruktorzyFitness { get; set; }
        public DbSet<Recepcjonista> Recepcjonisci { get; set; }
        public DbSet<Konserwator> Konserwatorzy { get; set; }
        public DbSet<Konserwacja> Konserwacje { get; set; }
        public DbSet<Sprzet> Sprzety { get; set; }
        public DbSet<TreningFitness> TreningiFitness { get; set; }
        public DbSet<TreningPersonalny> TreningiPersonalne { get; set; }
        public DbSet<TreningWlasny> TreningiWlasne { get; set; }
        public DbSet<KomentarzOPracowniku> KomentarzeOPracownikach { get; set; }
        public DbSet<Umowa> Umowy { get; set; }
        public DbSet<Sala> Sale { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //modelBuilder.Entity<Klient>().ToTable("Klienci");
            //modelBuilder.Entity<Mechanik>().ToTable("Mechanicy");
            //modelBuilder.Entity<Pracownik>().ToTable("Pracownicy");
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}