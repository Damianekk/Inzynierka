namespace Silownia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dodanieIDkówdoklasyUmowa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Umowa", "KlientID", c => c.Long(nullable: false));
            AddColumn("dbo.Umowa", "RecepcjonistaID", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Umowa", "RecepcjonistaID");
            DropColumn("dbo.Umowa", "KlientID");
        }
    }
}
