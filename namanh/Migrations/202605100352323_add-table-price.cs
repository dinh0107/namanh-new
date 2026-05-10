namespace namanh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtableprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceLangdings", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceLangdings", "Description");
        }
    }
}
