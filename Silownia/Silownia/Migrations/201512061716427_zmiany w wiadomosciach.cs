namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianywwiadomosciach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wiadomosc", "OsobaOdbierajaca_OsobaID", c => c.Long());
            AddColumn("dbo.Wiadomosc", "OsobaWysylajaca_OsobaID", c => c.Long());
            CreateIndex("dbo.Wiadomosc", "OsobaOdbierajaca_OsobaID");
            CreateIndex("dbo.Wiadomosc", "OsobaWysylajaca_OsobaID");
            AddForeignKey("dbo.Wiadomosc", "OsobaOdbierajaca_OsobaID", "dbo.Osoba", "OsobaID");
            AddForeignKey("dbo.Wiadomosc", "OsobaWysylajaca_OsobaID", "dbo.Osoba", "OsobaID");
            DropColumn("dbo.Wiadomosc", "OsobaWysylajaca");
            DropColumn("dbo.Wiadomosc", "OsobaOdbierajaca");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wiadomosc", "OsobaOdbierajaca", c => c.Long(nullable: false));
            AddColumn("dbo.Wiadomosc", "OsobaWysylajaca", c => c.Long(nullable: false));
            DropForeignKey("dbo.Wiadomosc", "OsobaWysylajaca_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.Wiadomosc", "OsobaOdbierajaca_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Wiadomosc", new[] { "OsobaWysylajaca_OsobaID" });
            DropIndex("dbo.Wiadomosc", new[] { "OsobaOdbierajaca_OsobaID" });
            DropColumn("dbo.Wiadomosc", "OsobaWysylajaca_OsobaID");
            DropColumn("dbo.Wiadomosc", "OsobaOdbierajaca_OsobaID");
        }
    }
}
