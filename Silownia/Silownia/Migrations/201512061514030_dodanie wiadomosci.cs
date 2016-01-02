namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodaniewiadomosci : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wiadomosc",
                c => new
                    {
                        WiadomoscID = c.Long(nullable: false, identity: true),
                        Tresc = c.String(),
                        OsobaWysylajaca = c.Long(nullable: false),
                        OsobaOdbierajaca = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        Wyslano = c.DateTime(),
                        Odebrano = c.DateTime(),
                    })
                .PrimaryKey(t => t.WiadomoscID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Wiadomosc");
        }
    }
}
