namespace hailinh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_pricelangding_image : DbMigration
    {
        public override void Up()
        {
            Sql(@"
IF COL_LENGTH('dbo.PriceLangdings', 'Image') IS NULL
    ALTER TABLE dbo.PriceLangdings ADD Image NVARCHAR(500) NULL;
");
        }
        
        public override void Down()
        {
            Sql(@"
IF COL_LENGTH('dbo.PriceLangdings', 'Image') IS NOT NULL
    ALTER TABLE dbo.PriceLangdings DROP COLUMN Image;
");
        }
    }
}
