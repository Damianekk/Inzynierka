namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pi3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Umowa", name: "Silownia_SilowniaID", newName: "SilowniaID");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.Umowa", name: "SilowniaID", newName: "Silownia_SilowniaID");
        }
    }
}
