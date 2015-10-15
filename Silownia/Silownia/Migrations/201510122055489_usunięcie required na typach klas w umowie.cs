namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usunięcierequirednatypachklaswumowie : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.Umowa", "Recepcjonista_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID" });
            DropIndex("dbo.Umowa", new[] { "Recepcjonista_OsobaID" });
            AlterColumn("dbo.Umowa", "Klient_OsobaID", c => c.Long());
            AlterColumn("dbo.Umowa", "Recepcjonista_OsobaID", c => c.Long());
            CreateIndex("dbo.Umowa", "Klient_OsobaID");
            CreateIndex("dbo.Umowa", "Recepcjonista_OsobaID");
            AddForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba", "OsobaID");
            AddForeignKey("dbo.Umowa", "Recepcjonista_OsobaID", "dbo.Osoba", "OsobaID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Umowa", "Recepcjonista_OsobaID", "dbo.Osoba");
            DropForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Umowa", new[] { "Recepcjonista_OsobaID" });
            DropIndex("dbo.Umowa", new[] { "Klient_OsobaID" });
            AlterColumn("dbo.Umowa", "Recepcjonista_OsobaID", c => c.Long(nullable: false));
            AlterColumn("dbo.Umowa", "Klient_OsobaID", c => c.Long(nullable: false));
            CreateIndex("dbo.Umowa", "Recepcjonista_OsobaID");
            CreateIndex("dbo.Umowa", "Klient_OsobaID");
            AddForeignKey("dbo.Umowa", "Recepcjonista_OsobaID", "dbo.Osoba", "OsobaID", cascadeDelete: true);
            AddForeignKey("dbo.Umowa", "Klient_OsobaID", "dbo.Osoba", "OsobaID", cascadeDelete: true);
        }
    }
}
