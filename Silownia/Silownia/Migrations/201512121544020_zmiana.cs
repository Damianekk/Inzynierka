namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmiana : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto");
            DropIndex("dbo.Osoba", new[] { "Konto_KontoID" });
            DropColumn("dbo.Osoba", "Konto_KontoID");
            DropTable("dbo.Konto");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Konto",
                c => new
                    {
                        KontoID = c.Long(nullable: false, identity: true),
                        Login = c.String(),
                        Haslo = c.String(),
                        DataUtworzenia = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.KontoID);
            
            AddColumn("dbo.Osoba", "Konto_KontoID", c => c.Long());
            CreateIndex("dbo.Osoba", "Konto_KontoID");
            AddForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto", "KontoID");
        }
    }
}
