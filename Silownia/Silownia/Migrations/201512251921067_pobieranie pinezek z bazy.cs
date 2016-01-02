namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pobieraniepinezekzbazy : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Silownia", "DodatkoweInfo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Silownia", "DodatkoweInfo");
        }
    }
}
