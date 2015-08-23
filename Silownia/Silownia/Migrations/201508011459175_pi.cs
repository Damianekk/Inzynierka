namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adres",
                c => new
                    {
                        AdresID = c.Long(nullable: false, identity: true),
                        KodPocztowy = c.String(),
                        Kraj = c.String(),
                        Miasto = c.String(),
                        Ulica = c.String(),
                        NrBudynku = c.String(),
                        NrLokalu = c.String(),
                    })
                .PrimaryKey(t => t.AdresID);
            
            CreateTable(
                "dbo.Osoba",
                c => new
                    {
                        OsobaID = c.Long(nullable: false, identity: true),
                        Imie = c.String(nullable: false),
                        Nazwisko = c.String(nullable: false),
                        DataUrodzenia = c.DateTime(nullable: false),
                        Mail = c.String(),
                        NrTelefonu = c.String(),
                        DataZatrudnienia = c.DateTime(),
                        Pensja = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Specjalizacja_SpecjalizacjaID = c.Long(),
                        Adres_AdresID = c.Long(),
                    })
                .PrimaryKey(t => t.OsobaID)
                .ForeignKey("dbo.Specjalizacja", t => t.Specjalizacja_SpecjalizacjaID, cascadeDelete: true)
                .ForeignKey("dbo.Adres", t => t.Adres_AdresID)
                .Index(t => t.Specjalizacja_SpecjalizacjaID)
                .Index(t => t.Adres_AdresID);
            
            CreateTable(
                "dbo.Masaz",
                c => new
                    {
                        MasazID = c.Long(nullable: false, identity: true),
                        DataMasazu = c.DateTime(nullable: false),
                        CzasTrwania = c.Int(nullable: false),
                        Klient_OsobaID = c.Long(nullable: false),
                        Masazystka_OsobaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.MasazID)
                .ForeignKey("dbo.Osoba", t => t.Klient_OsobaID, cascadeDelete: true)
                .ForeignKey("dbo.Osoba", t => t.Masazystka_OsobaID, cascadeDelete: false)
                .Index(t => t.Klient_OsobaID)
                .Index(t => t.Masazystka_OsobaID);
            
            CreateTable(
                "dbo.TreningPersonalny",
                c => new
                    {
                        TreningPersonalnyID = c.Long(nullable: false, identity: true),
                        DataTreningu = c.DateTime(nullable: false),
                        CzasTrwania = c.Int(nullable: false),
                        Klient_OsobaID = c.Long(nullable: false),
                        Trener_OsobaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.TreningPersonalnyID)
                .ForeignKey("dbo.Osoba", t => t.Klient_OsobaID, cascadeDelete: true)
                .ForeignKey("dbo.Osoba", t => t.Trener_OsobaID, cascadeDelete: false)
                .Index(t => t.Klient_OsobaID)
                .Index(t => t.Trener_OsobaID);
            
            CreateTable(
                "dbo.Specjalizacja",
                c => new
                    {
                        SpecjalizacjaID = c.Long(nullable: false, identity: true),
                        Nazwa = c.String(),
                    })
                .PrimaryKey(t => t.SpecjalizacjaID);
            
            CreateTable(
                "dbo.Umowa",
                c => new
                    {
                        UmowaID = c.Long(nullable: false, identity: true),
                        DataPodpisania = c.DateTime(nullable: false),
                        DataZakonczenia = c.DateTime(nullable: false),
                        Cena = c.String(),
                        Klient_OsobaID = c.Long(nullable: false),
                        Silownia_SilowniaID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UmowaID)
                .ForeignKey("dbo.Osoba", t => t.Klient_OsobaID, cascadeDelete: true)
                .ForeignKey("dbo.Silownia", t => t.Silownia_SilowniaID, cascadeDelete: true)
                .Index(t => t.Klient_OsobaID)
                .Index(t => t.Silownia_SilowniaID);
            
            CreateTable(
                "dbo.Silownia",
                c => new
                    {
                        SilowniaID = c.Long(nullable: false, identity: true),
                        Nazwa = c.String(),
                        GodzinaOtwarcia = c.String(),
                        GodzinaZamkniecia = c.String(),
                        Powierzchnia = c.String(),
                        NrTelefonu = c.Long(nullable: false),
                        Adres_AdresID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SilowniaID)
                .ForeignKey("dbo.Adres", t => t.Adres_AdresID, cascadeDelete: true)
                .Index(t => t.Adres_AdresID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Osoba", "Adres_AdresID", "dbo.Adres");
            DropForeignKey("dbo.Umowa", "Silownia_SilowniaID", "dbo.Silownia");
            DropForeignKey("dbo.Silownia", "Adres_AdresID", "dbo.Adres");
            DropForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.TreningPersonalny", "Trener_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.Osoba", "Specjalizacja_SpecjalizacjaID", "dbo.Specjalizacja");
            DropForeignKey("dbo.TreningPersonalny", "Klient_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.Masaz", "Masazystka_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.Masaz", "Klient_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Osoba", new[] { "Adres_AdresID" });
            DropIndex("dbo.Umowa", new[] { "Silownia_SilowniaID" });
            DropIndex("dbo.Silownia", new[] { "Adres_AdresID" });
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID" });
            DropIndex("dbo.TreningPersonalny", new[] { "Trener_OsobaID" });
            DropIndex("dbo.Osoba", new[] { "Specjalizacja_SpecjalizacjaID" });
            DropIndex("dbo.TreningPersonalny", new[] { "Klient_OsobaID" });
            DropIndex("dbo.Masaz", new[] { "Masazystka_OsobaID" });
            DropIndex("dbo.Masaz", new[] { "Klient_OsobaID" });
            DropTable("dbo.Silownia");
            DropTable("dbo.Umowa");
            DropTable("dbo.Specjalizacja");
            DropTable("dbo.TreningPersonalny");
            DropTable("dbo.Masaz");
            DropTable("dbo.Osoba");
            DropTable("dbo.Adres");
        }
    }
}
