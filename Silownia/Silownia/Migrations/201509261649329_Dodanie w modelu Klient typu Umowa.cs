namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DodaniewmodeluKlienttypuUmowa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba");
            AddColumn("dbo.Osoba", "UmowaKlient_UmowaID", c => c.Long());
            AddColumn("dbo.Umowa", "Klient_OsobaID1", c => c.Long());
            CreateIndex("dbo.Osoba", "UmowaKlient_UmowaID");
            CreateIndex("dbo.Umowa", "Klient_OsobaID1");
            AddForeignKey("dbo.Osoba", "UmowaKlient_UmowaID", "dbo.Umowa", "UmowaID");
            AddForeignKey("dbo.Umowa", "Klient_OsobaID1", "dbo.Osoba", "OsobaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Umowa", "Klient_OsobaID1", "dbo.Osoba");
            DropForeignKey("dbo.Osoba", "UmowaKlient_UmowaID", "dbo.Umowa");
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID1" });
            DropIndex("dbo.Osoba", new[] { "UmowaKlient_UmowaID" });
            DropColumn("dbo.Umowa", "Klient_OsobaID1");
            DropColumn("dbo.Osoba", "UmowaKlient_UmowaID");
            AddForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba", "OsobaID", cascadeDelete: true);
        }
    }
}
