namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Dodaniemozliwosciutworzeniakonta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Konto",
                c => new
                    {
                        KontoID = c.Long(nullable: false),
                        Login = c.String(),
                        Haslo = c.String(),
                        DataUtworzenia = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.KontoID);
            
            AddColumn("dbo.Osoba", "Konto_KontoID", c => c.Long());
            CreateIndex("dbo.Osoba", "Konto_KontoID");
            AddForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto", "KontoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto");
            DropIndex("dbo.Osoba", new[] { "Konto_KontoID" });
            DropColumn("dbo.Osoba", "Konto_KontoID");
            DropTable("dbo.Konto");
        }
    }
}
