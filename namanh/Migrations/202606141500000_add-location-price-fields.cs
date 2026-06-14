namespace namanh.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class addlocationpricefields : DbMigration
    {
        public override void Up()
        {
            Sql(@"
IF COL_LENGTH('dbo.Locations', 'Price4') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Price4 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price7 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price16 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price29 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Price45 NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD PriceLim NVARCHAR(MAX) NULL;
    ALTER TABLE dbo.Locations ADD Sort INT NOT NULL CONSTRAINT DF_Locations_Sort DEFAULT 1;
    ALTER TABLE dbo.Locations ADD Hot BIT NOT NULL CONSTRAINT DF_Locations_Hot DEFAULT 0;
    ALTER TABLE dbo.Locations ADD Active BIT NOT NULL CONSTRAINT DF_Locations_Active DEFAULT 1;
END
");
        }

        public override void Down()
        {
            Sql(@"
IF COL_LENGTH('dbo.Locations', 'Price4') IS NOT NULL
BEGIN
    IF OBJECT_ID('DF_Locations_Sort', 'D') IS NOT NULL ALTER TABLE dbo.Locations DROP CONSTRAINT DF_Locations_Sort;
    IF OBJECT_ID('DF_Locations_Hot', 'D') IS NOT NULL ALTER TABLE dbo.Locations DROP CONSTRAINT DF_Locations_Hot;
    IF OBJECT_ID('DF_Locations_Active', 'D') IS NOT NULL ALTER TABLE dbo.Locations DROP CONSTRAINT DF_Locations_Active;
    ALTER TABLE dbo.Locations DROP COLUMN Price4, Price7, Price16, Price29, Price45, PriceLim, Sort, Hot, Active;
END
");
        }
    }
}
