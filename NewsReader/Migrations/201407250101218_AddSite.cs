namespace NewsReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsItems", "Site", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewsItems", "Site");
        }
    }
}
