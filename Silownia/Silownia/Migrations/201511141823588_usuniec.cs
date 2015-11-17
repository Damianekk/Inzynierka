namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usuniec : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Umowa", "Cena", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Umowa", "Cena", c => c.String());
        }
    }
}
