namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodaniewiadomoscidoosoby : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wiadomosc", "Osoba_OsobaID", c => c.Long());
            CreateIndex("dbo.Wiadomosc", "Osoba_OsobaID");
            AddForeignKey("dbo.Wiadomosc", "Osoba_OsobaID", "dbo.Osoba", "OsobaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wiadomosc", "Osoba_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Wiadomosc", new[] { "Osoba_OsobaID" });
            DropColumn("dbo.Wiadomosc", "Osoba_OsobaID");
        }
    }
}
