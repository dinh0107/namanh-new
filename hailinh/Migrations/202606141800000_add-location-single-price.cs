namespace hailinh.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addlocationsingleprice : DbMigration
    {
        public override void Up()
        {
            // Tách 2 lệnh Sql riêng — SQL Server compile cả batch trước khi chạy IF,
            // nên UPDATE Price trong cùng batch với ALTER TABLE sẽ báo Invalid column name.
            Sql(@"
IF COL_LENGTH('dbo.Locations', 'Price') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Price NVARCHAR(MAX) NULL;
END
");

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
            Sql(@"
IF COL_LENGTH('dbo.Locations', 'Price') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Locations DROP COLUMN Price;
END
");
        }
    }
}
