namespace namanh.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addlocationpricefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Price4", c => c.String());
            AddColumn("dbo.Locations", "Price7", c => c.String());
            AddColumn("dbo.Locations", "Price16", c => c.String());
            AddColumn("dbo.Locations", "Price29", c => c.String());
            AddColumn("dbo.Locations", "Price45", c => c.String());
            AddColumn("dbo.Locations", "PriceLim", c => c.String());
            AddColumn("dbo.Locations", "Sort", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.Locations", "Hot", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.Locations", "Active", c => c.Boolean(nullable: false, defaultValue: true));
        }

        public override void Down()
        {
            DropColumn("dbo.Locations", "Active");
            DropColumn("dbo.Locations", "Hot");
            DropColumn("dbo.Locations", "Sort");
            DropColumn("dbo.Locations", "PriceLim");
            DropColumn("dbo.Locations", "Price45");
            DropColumn("dbo.Locations", "Price29");
            DropColumn("dbo.Locations", "Price16");
            DropColumn("dbo.Locations", "Price7");
            DropColumn("dbo.Locations", "Price4");
        }
    }
}
