namespace LightBlue.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CloudBlobContainers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlobDirectories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Uri = c.String(),
                        CloudBlobContainerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudBlobContainers", t => t.CloudBlobContainerId, cascadeDelete: true)
                .Index(t => t.CloudBlobContainerId);
            
            CreateTable(
                "dbo.CloudBlobs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Content = c.String(),
                        ContentType = c.String(),
                        Length = c.Int(nullable: false),
                        BlobDirectoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlobDirectories", t => t.BlobDirectoryId, cascadeDelete: true)
                .Index(t => t.BlobDirectoryId);
            
            CreateTable(
                "dbo.CloudQueues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CloudQueueMessages",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Content = c.String(),
                        CloudQueueId = c.Int(nullable: false),
                        CloudQueue_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudQueues", t => t.CloudQueue_Id)
                .Index(t => t.CloudQueue_Id);
            
            CreateTable(
                "dbo.CloudServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CloudRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assembly = c.String(),
                        Name = c.String(),
                        Instances = c.Int(nullable: false),
                        CloudServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudServices", t => t.CloudServiceId, cascadeDelete: true)
                .Index(t => t.CloudServiceId);
            
            CreateTable(
                "dbo.ConfigurationSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        CloudRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudRoles", t => t.CloudRoleId, cascadeDelete: true)
                .Index(t => t.CloudRoleId);
            
            CreateTable(
                "dbo.LocalResouces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Content = c.String(),
                        CloudRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudRoles", t => t.CloudRoleId, cascadeDelete: true)
                .Index(t => t.CloudRoleId);
            
            CreateTable(
                "dbo.RoleLogs",
                c => new
                    {
                        RoleLogId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CloudRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleLogId)
                .ForeignKey("dbo.CloudRoles", t => t.CloudRoleId, cascadeDelete: true)
                .Index(t => t.CloudRoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleLogs", "CloudRoleId", "dbo.CloudRoles");
            DropForeignKey("dbo.LocalResouces", "CloudRoleId", "dbo.CloudRoles");
            DropForeignKey("dbo.ConfigurationSettings", "CloudRoleId", "dbo.CloudRoles");
            DropForeignKey("dbo.CloudRoles", "CloudServiceId", "dbo.CloudServices");
            DropForeignKey("dbo.CloudQueueMessages", "CloudQueue_Id", "dbo.CloudQueues");
            DropForeignKey("dbo.BlobDirectories", "CloudBlobContainerId", "dbo.CloudBlobContainers");
            DropForeignKey("dbo.CloudBlobs", "BlobDirectoryId", "dbo.BlobDirectories");
            DropIndex("dbo.RoleLogs", new[] { "CloudRoleId" });
            DropIndex("dbo.LocalResouces", new[] { "CloudRoleId" });
            DropIndex("dbo.ConfigurationSettings", new[] { "CloudRoleId" });
            DropIndex("dbo.CloudRoles", new[] { "CloudServiceId" });
            DropIndex("dbo.CloudQueueMessages", new[] { "CloudQueue_Id" });
            DropIndex("dbo.CloudBlobs", new[] { "BlobDirectoryId" });
            DropIndex("dbo.BlobDirectories", new[] { "CloudBlobContainerId" });
            DropTable("dbo.RoleLogs");
            DropTable("dbo.LocalResouces");
            DropTable("dbo.ConfigurationSettings");
            DropTable("dbo.CloudRoles");
            DropTable("dbo.CloudServices");
            DropTable("dbo.CloudQueueMessages");
            DropTable("dbo.CloudQueues");
            DropTable("dbo.CloudBlobs");
            DropTable("dbo.BlobDirectories");
            DropTable("dbo.CloudBlobContainers");
        }
    }
}
