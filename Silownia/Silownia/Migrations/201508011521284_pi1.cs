namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pi1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Osoba", name: "Specjalizacja_SpecjalizacjaID", newName: "SpecjalizacjaID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Osoba", name: "SpecjalizacjaID", newName: "Specjalizacja_SpecjalizacjaID");
        }
    }
}
