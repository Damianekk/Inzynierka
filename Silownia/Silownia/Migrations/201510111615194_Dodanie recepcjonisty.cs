namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodanierecepcjonisty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Umowa", "Recepcjonista_OsobaID", c => c.Long(nullable: false));
            CreateIndex("dbo.Umowa", "Recepcjonista_OsobaID");
            AddForeignKey("dbo.Umowa", "Recepcjonista_OsobaID", "dbo.Osoba", "OsobaID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Umowa", "Recepcjonista_OsobaID", "dbo.Osoba");
            DropIndex("dbo.Umowa", new[] { "Recepcjonista_OsobaID" });
            DropColumn("dbo.Umowa", "Recepcjonista_OsobaID");
        }
    }
}
