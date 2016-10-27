namespace LightBlue.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameBlobs1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CloudServices", "ServiceDefinition", c => c.String());
            AddColumn("dbo.CloudServices", "ServiceConfiguration", c => c.String());
            AddColumn("dbo.CloudRoles", "IsEnabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CloudRoles", "IsEnabled");
            DropColumn("dbo.CloudServices", "ServiceConfiguration");
            DropColumn("dbo.CloudServices", "ServiceDefinition");
        }
    }
}
