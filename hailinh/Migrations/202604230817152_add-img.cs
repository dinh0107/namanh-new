namespace hailinh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimg : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConfigSites", "FooterImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConfigSites", "FooterImage");
        }
    }
}
