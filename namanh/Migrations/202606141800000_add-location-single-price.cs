namespace namanh.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addlocationsingleprice : DbMigration
    {
        public override void Up()
        {
            if (!ColumnExists("dbo.Locations", "Price"))
            {
                AddColumn("dbo.Locations", "Price", c => c.String());
            }

            Sql(@"
IF COL_LENGTH('dbo.Locations', 'Price4') IS NOT NULL
BEGIN
    UPDATE dbo.Locations
    SET Price = COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim)
    WHERE COALESCE(NULLIF(LTRIM(RTRIM(Price)), ''), Price4, Price7, Price16, Price29, Price45, PriceLim) IS NOT NULL;
END
");
        }

        public override void Down()
        {
            if (ColumnExists("dbo.Locations", "Price"))
            {
                DropColumn("dbo.Locations", "Price");
            }
        }
    }
}
