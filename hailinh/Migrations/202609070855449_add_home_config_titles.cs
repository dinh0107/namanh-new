namespace hailinh.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_home_config_titles : DbMigration
    {
        public override void Up()
        {
            // Idempotent: LocalDB có thể đã có cột từ lần ALTER tay.
            Sql(@"
IF COL_LENGTH('dbo.ConfigSites', 'HomeFleetTitle') IS NULL
BEGIN
    ALTER TABLE dbo.ConfigSites ADD HomeFleetTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeFleetDesc NVARCHAR(500) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomePriceTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomePriceDesc NVARCHAR(500) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomePriceNoteTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeCtaTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeCtaDesc NVARCHAR(500) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeReviewTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeNewsTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeNewsDesc NVARCHAR(500) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeFaqTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeFaqCtaTitle NVARCHAR(200) NULL;
    ALTER TABLE dbo.ConfigSites ADD HomeFaqCtaDesc NVARCHAR(500) NULL;
END
");
        }

        public override void Down()
        {
            Sql(@"
IF COL_LENGTH('dbo.ConfigSites', 'HomeFleetTitle') IS NOT NULL
BEGIN
    ALTER TABLE dbo.ConfigSites DROP COLUMN
        HomeFleetTitle, HomeFleetDesc,
        HomePriceTitle, HomePriceDesc, HomePriceNoteTitle,
        HomeCtaTitle, HomeCtaDesc,
        HomeReviewTitle,
        HomeNewsTitle, HomeNewsDesc,
        HomeFaqTitle, HomeFaqCtaTitle, HomeFaqCtaDesc;
END
");
        }
    }
}
