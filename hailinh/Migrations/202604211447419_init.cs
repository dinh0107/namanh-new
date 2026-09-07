namespace hailinh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false, maxLength: 60),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Url = c.String(maxLength: 500),
                        CategorySort = c.Int(nullable: false),
                        CategoryActive = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Redirect = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        ShowMenu = c.Boolean(nullable: false),
                        ShowFooter = c.Boolean(nullable: false),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        Image = c.String(maxLength: 500),
                        TypePost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false, maxLength: 500),
                        Body = c.String(),
                        Image = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Draft = c.Boolean(nullable: false),
                        View = c.Int(nullable: false),
                        ArticleCategoryId = c.Int(nullable: false),
                        CarServiceId = c.Int(),
                        Active = c.Boolean(nullable: false),
                        ShowMenu = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 300),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategoryId, cascadeDelete: true)
                .ForeignKey("dbo.CarServices", t => t.CarServiceId)
                .Index(t => t.ArticleCategoryId)
                .Index(t => t.CarServiceId);
            
            CreateTable(
                "dbo.CarServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Slogan = c.String(),
                        Car = c.String(),
                        Slug = c.String(),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        ImageUrlMobile = c.String(),
                        Image = c.String(),
                        Firm = c.String(),
                        Capacity = c.String(),
                        Type = c.String(),
                        Speed = c.String(),
                        ListImage = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Home = c.Boolean(nullable: false),
                        Menu = c.Boolean(nullable: false),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                        Body = c.String(),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CarServicePrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarServiceId = c.Int(nullable: false),
                        RouteDescription = c.String(nullable: false),
                        Price = c.String(nullable: false),
                        Km = c.String(),
                        Hot = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarServices", t => t.CarServiceId, cascadeDelete: true)
                .Index(t => t.CarServiceId);
            
            CreateTable(
                "dbo.CarServiceDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarServiceId = c.Int(nullable: false),
                        Image = c.String(),
                        ListImage = c.String(),
                        Name = c.String(nullable: false),
                        Desciption = c.String(),
                        Sort = c.Int(nullable: false),
                        Body = c.String(),
                        CarServiceType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CarServices", t => t.CarServiceId, cascadeDelete: true)
                .Index(t => t.CarServiceId);
            
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerName = c.String(nullable: false, maxLength: 100),
                        Slogan = c.String(maxLength: 2000),
                        Image = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        Sort = c.Int(nullable: false),
                        Content = c.String(),
                        ListImage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Facebook = c.String(maxLength: 500),
                        Youtube = c.String(maxLength: 500),
                        Twitter = c.String(maxLength: 500),
                        Instagram = c.String(maxLength: 500),
                        Messenger = c.String(maxLength: 500),
                        Image = c.String(),
                        ImageShare = c.String(),
                        Car4 = c.String(),
                        Car7 = c.String(),
                        Car16 = c.String(),
                        Favicon = c.String(),
                        GoogleMap = c.String(maxLength: 4000),
                        GoogleAnalytics = c.String(maxLength: 4000),
                        Place = c.String(),
                        Title = c.String(maxLength: 200),
                        AboutImage = c.String(),
                        AboutMission = c.String(),
                        AboutText = c.String(),
                        AboutUrl = c.String(maxLength: 500),
                        Description = c.String(maxLength: 500),
                        Hotline = c.String(maxLength: 50),
                        Hotline2 = c.String(maxLength: 50),
                        Zalo = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        InfoContact = c.String(),
                        InfoFooter = c.String(),
                        Price = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(nullable: false, maxLength: 200),
                        To = c.String(nullable: false, maxLength: 200),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(),
                        Mobile = c.String(nullable: false, maxLength: 10),
                        CreateDate = c.DateTime(nullable: false),
                        StatusContact = c.Int(nullable: false),
                        TypeCar = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Mobile = c.String(nullable: false, maxLength: 10),
                        Code = c.String(nullable: false, maxLength: 50),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(nullable: false, maxLength: 200),
                        To = c.String(nullable: false, maxLength: 200),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(),
                        Distance = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        PriceChange = c.Decimal(precision: 18, scale: 2),
                        PriceSale = c.Decimal(precision: 18, scale: 2),
                        Tolls = c.Decimal(precision: 18, scale: 2),
                        Other = c.Decimal(precision: 18, scale: 2),
                        Pile = c.Decimal(precision: 18, scale: 2),
                        Bill = c.Decimal(precision: 18, scale: 2),
                        IsBillPaid = c.Boolean(nullable: false),
                        Note = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ClosingMonth = c.DateTime(),
                        Status = c.Int(nullable: false),
                        TypeCar = c.Int(nullable: false),
                        TypeTrip = c.Int(nullable: false),
                        Source = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DriverId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.CustomerId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        QRCode = c.String(maxLength: 500),
                        Active = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        CountUsed = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        CustomerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Expenditure = c.Int(nullable: false),
                        Price = c.Decimal(precision: 18, scale: 2),
                        CreateDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Note = c.String(maxLength: 500),
                        DriverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Introduces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(maxLength: 500),
                        AboutText = c.String(),
                        StaffText = c.String(),
                        FeedbackText = c.String(),
                        TitleMeta = c.String(maxLength: 100),
                        DescriptionMeta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PriceTables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarServiceId = c.Int(nullable: false),
                        RouteDescription = c.String(nullable: false),
                        Price4 = c.String(nullable: false),
                        Price7 = c.String(nullable: false),
                        Price16 = c.String(nullable: false),
                        Price29 = c.String(nullable: false),
                        Price45 = c.String(nullable: false),
                        PriceLim = c.String(nullable: false),
                        Hot = c.Boolean(nullable: false),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Expenses", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Vouchers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Trips", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Trips", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Articles", "CarServiceId", "dbo.CarServices");
            DropForeignKey("dbo.CarServiceDetails", "CarServiceId", "dbo.CarServices");
            DropForeignKey("dbo.CarServicePrices", "CarServiceId", "dbo.CarServices");
            DropForeignKey("dbo.Articles", "ArticleCategoryId", "dbo.ArticleCategories");
            DropIndex("dbo.Expenses", new[] { "DriverId" });
            DropIndex("dbo.Vouchers", new[] { "CustomerId" });
            DropIndex("dbo.Trips", new[] { "DriverId" });
            DropIndex("dbo.Trips", new[] { "CustomerId" });
            DropIndex("dbo.CarServiceDetails", new[] { "CarServiceId" });
            DropIndex("dbo.CarServicePrices", new[] { "CarServiceId" });
            DropIndex("dbo.Articles", new[] { "CarServiceId" });
            DropIndex("dbo.Articles", new[] { "ArticleCategoryId" });
            DropTable("dbo.PriceTables");
            DropTable("dbo.Introduces");
            DropTable("dbo.Expenses");
            DropTable("dbo.Vouchers");
            DropTable("dbo.Drivers");
            DropTable("dbo.Trips");
            DropTable("dbo.Customers");
            DropTable("dbo.Contacts");
            DropTable("dbo.ConfigSites");
            DropTable("dbo.Banners");
            DropTable("dbo.CarServiceDetails");
            DropTable("dbo.CarServicePrices");
            DropTable("dbo.CarServices");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleCategories");
            DropTable("dbo.Admins");
        }
    }
}
