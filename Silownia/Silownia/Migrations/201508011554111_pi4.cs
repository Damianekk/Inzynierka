namespace Silownia.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class pi4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Osoba", "AdresID_AdresID", c => c.Long());
            CreateIndex("dbo.Osoba", "AdresID_AdresID");
            AddForeignKey("dbo.Osoba", "AdresID_AdresID", "dbo.Adres", "AdresID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Osoba", "AdresID_AdresID", "dbo.Adres");
            DropIndex("dbo.Osoba", new[] { "AdresID_AdresID" });
            DropColumn("dbo.Osoba", "AdresID_AdresID");
        }
    }
}
