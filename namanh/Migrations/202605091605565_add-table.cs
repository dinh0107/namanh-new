namespace namanh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        PriceLangdingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceLangdings", t => t.PriceLangdingId, cascadeDelete: true)
                .Index(t => t.PriceLangdingId);
            
            CreateTable(
                "dbo.PriceLangdings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Sort = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Locations", "PriceLangdingId", "dbo.PriceLangdings");
            DropIndex("dbo.Locations", new[] { "PriceLangdingId" });
            DropTable("dbo.PriceLangdings");
            DropTable("dbo.Locations");
        }
    }
}
