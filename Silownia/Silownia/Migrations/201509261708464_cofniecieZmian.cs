namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cofniecieZmian : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Osoba", "UmowaKlient_UmowaID", "dbo.Umowa");
            DropForeignKey("dbo.Umowa", "Klient_OsobaID1", "dbo.Osoba");
            DropIndex("dbo.Osoba", new[] { "UmowaKlient_UmowaID" });
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID" });
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID1" });
            DropColumn("dbo.Umowa", "Klient_OsobaID");
            RenameColumn(table: "dbo.Umowa", name: "Klient_OsobaID1", newName: "Klient_OsobaID");
            AlterColumn("dbo.Umowa", "Klient_OsobaID", c => c.Long(nullable: false));
            CreateIndex("dbo.Umowa", "Klient_OsobaID");
            AddForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba", "OsobaID", cascadeDelete: true);
            DropColumn("dbo.Osoba", "UmowaKlient_UmowaID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Osoba", "UmowaKlient_UmowaID", c => c.Long());
            DropForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID" });
            AlterColumn("dbo.Umowa", "Klient_OsobaID", c => c.Long());
            RenameColumn(table: "dbo.Umowa", name: "Klient_OsobaID", newName: "Klient_OsobaID1");
            AddColumn("dbo.Umowa", "Klient_OsobaID", c => c.Long(nullable: false));
            CreateIndex("dbo.Umowa", "Klient_OsobaID1");
            CreateIndex("dbo.Umowa", "Klient_OsobaID");
            CreateIndex("dbo.Osoba", "UmowaKlient_UmowaID");
            AddForeignKey("dbo.Umowa", "Klient_OsobaID1", "dbo.Osoba", "OsobaID");
            AddForeignKey("dbo.Osoba", "UmowaKlient_UmowaID", "dbo.Umowa", "UmowaID");
        }
    }
}
