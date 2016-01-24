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
            //  Database.SetInitializer(new MigrateDatabaseToLatestVersion<SilowniaContext, Silownia.Migrations.Configuration>("SilowniaContext"));
        }

        public DbSet<Osoba> Osoby { get; set; }
        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<Trening> Treningi { get; set; }
        public DbSet<Adres> Adresy { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Masaz> Masaze { get; set; }
        public DbSet<Masazysta> Masazysci { get; set; }
        public DbSet<Models.Silownia> Silownie { get; set; }
        public DbSet<Specjalizacja> Specjalizacje { get; set; }
        public DbSet<Trener> Trenerzy { get; set; }
        public DbSet<Instruktor> Instruktorzy { get; set; }
        public DbSet<Recepcjonista> Recepcjonisci { get; set; }
        public DbSet<Konserwator> Konserwatorzy { get; set; }
        public DbSet<Konserwacja> Konserwacje { get; set; }
        public DbSet<Sprzet> Sprzety { get; set; }
        public DbSet<ZajeciaGrupowe> ZajeciaGrup { get; set; }
        public DbSet<TreningPersonalny> TreningiPersonalne { get; set; }
        public DbSet<Umowa> Umowy { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Sala> Sale { get; set; }
        public DbSet<Wiadomosc> Wiadomosci { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new DziedziczenieOsoby());
            modelBuilder.Configurations.Add(new DziedziczeniePracownika());
            modelBuilder.Configurations.Add(new DziedziczenieTreningu());

            //Usuwanie kaskadowe miedzy tabelami powiazanymi ze sobą
            modelBuilder.Entity<Models.Silownia>()
                        .HasMany(t => t.Sale)
                        .WithRequired(t=>t.Silownia)
                        .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Models.Silownia>()
                        .HasMany(t => t.Pracownicy)
                        .WithRequired(t => t.Silownia)
                        .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Models.Silownia>()
                        .HasMany(t => t.Umowy)
                        .WithRequired(t => t.Silownia)
                        .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Trener>()
                       .HasMany(t => t.TreningiPersonalne)
                       .WithRequired(t => t.Trener)
                       .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Masazysta>()
                       .HasMany(t => t.Masaze)
                       .WithRequired(t => t.Masazysta)
                       .WillCascadeOnDelete(true);
            
   
            modelBuilder.Entity<Sala>()
                        .HasMany(t => t.Sprzety)
                        .WithRequired(t => t.Sala)
                        .WillCascadeOnDelete(true);
            
            modelBuilder.Entity<Sala>()
                        .HasMany(t => t.ZajeciaGrup)
                        .WithRequired(t => t.Sala)
                        .WillCascadeOnDelete(true);
                        

        }

    }
}