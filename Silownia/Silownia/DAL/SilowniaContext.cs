using Silownia.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Silownia.DAL
{
    public class SilowniaContext : DbContext
    {
        public SilowniaContext()
            : base("SilowniaContext")
        {
        }

        public DbSet<Adres> Adresy { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Masaz> Masaze { get; set; }
        public DbSet<Masazystka> Masazysci { get; set; }
        public DbSet<Osoba> Osoby { get; set; }
        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<Models.Silownia> Silownie { get; set; }
        public DbSet<Specjalizacja> Specjalizacje { get; set; }
        public DbSet<Trener> Trenerzy { get; set; }
        public DbSet<TreningPersonalny> TreningiPersonalne { get; set; }
        public DbSet<Umowa> Umowy { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<Klient>().ToTable("Klienci");
            //modelBuilder.Entity<Mechanik>().ToTable("Mechanicy");
            //modelBuilder.Entity<Pracownik>().ToTable("Pracownicy");

        }

    }
}