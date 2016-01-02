namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zmianawklasiekonto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto");
            DropPrimaryKey("dbo.Konto");
            AlterColumn("dbo.Konto", "KontoID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Konto", "KontoID");
            AddForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto", "KontoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto");
            DropPrimaryKey("dbo.Konto");
            AlterColumn("dbo.Konto", "KontoID", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Konto", "KontoID");
            AddForeignKey("dbo.Osoba", "Konto_KontoID", "dbo.Konto", "KontoID");
        }
    }
}
