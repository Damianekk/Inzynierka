namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pi2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Silownia", "Adres_AdresID", "dbo.Adres");
            DropIndex("dbo.Silownia", new[] { "Adres_AdresID" });
            AlterColumn("dbo.Silownia", "Adres_AdresID", c => c.Long());
            CreateIndex("dbo.Silownia", "Adres_AdresID");
            AddForeignKey("dbo.Silownia", "Adres_AdresID", "dbo.Adres", "AdresID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Silownia", "Adres_AdresID", "dbo.Adres");
            DropIndex("dbo.Silownia", new[] { "Adres_AdresID" });
            AlterColumn("dbo.Silownia", "Adres_AdresID", c => c.Long(nullable: false));
            CreateIndex("dbo.Silownia", "Adres_AdresID");
            AddForeignKey("dbo.Silownia", "Adres_AdresID", "dbo.Adres", "AdresID", cascadeDelete: true);
        }
    }
}
