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
                "dbo.CloudServices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        VmSize = c.String(),
                        ServiceDefinition = c.String(),
                        ServiceConfiguration = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assembly = c.String(),
                        Name = c.String(),
                        Instances = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
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
                        RoleId = c.Int(nullable: false),
                        WebRole_Id = c.Int(),
                        WorkerRole_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebRoles", t => t.WebRole_Id)
                .ForeignKey("dbo.WorkerRoles", t => t.WorkerRole_Id)
                .Index(t => t.WebRole_Id)
                .Index(t => t.WorkerRole_Id);
            
            CreateTable(
                "dbo.InputEndpoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Protocol = c.String(),
                        Port = c.Int(nullable: false),
                        CloudRoleId = c.Int(nullable: false),
                        WebRole_Id = c.Int(),
                        WorkerRole_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebRoles", t => t.WebRole_Id)
                .ForeignKey("dbo.WorkerRoles", t => t.WorkerRole_Id)
                .Index(t => t.WebRole_Id)
                .Index(t => t.WorkerRole_Id);
            
            CreateTable(
                "dbo.LocalResouces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Content = c.String(),
                        CloudRoleId = c.Int(nullable: false),
                        WebRole_Id = c.Int(),
                        WorkerRole_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebRoles", t => t.WebRole_Id)
                .ForeignKey("dbo.WorkerRoles", t => t.WorkerRole_Id)
                .Index(t => t.WebRole_Id)
                .Index(t => t.WorkerRole_Id);
            
            CreateTable(
                "dbo.RoleLogs",
                c => new
                    {
                        RoleLogId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CloudRoleId = c.Int(nullable: false),
                        WebRole_Id = c.Int(),
                        WorkerRole_Id = c.Int(),
                    })
                .PrimaryKey(t => t.RoleLogId)
                .ForeignKey("dbo.WebRoles", t => t.WebRole_Id)
                .ForeignKey("dbo.WorkerRoles", t => t.WorkerRole_Id)
                .Index(t => t.WebRole_Id)
                .Index(t => t.WorkerRole_Id);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WebRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WebRoles", t => t.WebRoleId, cascadeDelete: true)
                .Index(t => t.WebRoleId);
            
            CreateTable(
                "dbo.Bindings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EndPointName = c.String(),
                        SiteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.WorkerRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assembly = c.String(),
                        Name = c.String(),
                        Instances = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        CloudServiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CloudServices", t => t.CloudServiceId, cascadeDelete: true)
                .Index(t => t.CloudServiceId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CloudQueueMessages", "CloudQueue_Id", "dbo.CloudQueues");
            DropForeignKey("dbo.RoleLogs", "WorkerRole_Id", "dbo.WorkerRoles");
            DropForeignKey("dbo.LocalResouces", "WorkerRole_Id", "dbo.WorkerRoles");
            DropForeignKey("dbo.InputEndpoints", "WorkerRole_Id", "dbo.WorkerRoles");
            DropForeignKey("dbo.ConfigurationSettings", "WorkerRole_Id", "dbo.WorkerRoles");
            DropForeignKey("dbo.WorkerRoles", "CloudServiceId", "dbo.CloudServices");
            DropForeignKey("dbo.Sites", "WebRoleId", "dbo.WebRoles");
            DropForeignKey("dbo.Bindings", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.RoleLogs", "WebRole_Id", "dbo.WebRoles");
            DropForeignKey("dbo.LocalResouces", "WebRole_Id", "dbo.WebRoles");
            DropForeignKey("dbo.InputEndpoints", "WebRole_Id", "dbo.WebRoles");
            DropForeignKey("dbo.ConfigurationSettings", "WebRole_Id", "dbo.WebRoles");
            DropForeignKey("dbo.WebRoles", "CloudServiceId", "dbo.CloudServices");
            DropForeignKey("dbo.BlobDirectories", "CloudBlobContainerId", "dbo.CloudBlobContainers");
            DropForeignKey("dbo.CloudBlobs", "BlobDirectoryId", "dbo.BlobDirectories");
            DropIndex("dbo.CloudQueueMessages", new[] { "CloudQueue_Id" });
            DropIndex("dbo.WorkerRoles", new[] { "CloudServiceId" });
            DropIndex("dbo.Bindings", new[] { "SiteId" });
            DropIndex("dbo.Sites", new[] { "WebRoleId" });
            DropIndex("dbo.RoleLogs", new[] { "WorkerRole_Id" });
            DropIndex("dbo.RoleLogs", new[] { "WebRole_Id" });
            DropIndex("dbo.LocalResouces", new[] { "WorkerRole_Id" });
            DropIndex("dbo.LocalResouces", new[] { "WebRole_Id" });
            DropIndex("dbo.InputEndpoints", new[] { "WorkerRole_Id" });
            DropIndex("dbo.InputEndpoints", new[] { "WebRole_Id" });
            DropIndex("dbo.ConfigurationSettings", new[] { "WorkerRole_Id" });
            DropIndex("dbo.ConfigurationSettings", new[] { "WebRole_Id" });
            DropIndex("dbo.WebRoles", new[] { "CloudServiceId" });
            DropIndex("dbo.CloudBlobs", new[] { "BlobDirectoryId" });
            DropIndex("dbo.BlobDirectories", new[] { "CloudBlobContainerId" });
            DropTable("dbo.CloudQueueMessages");
            DropTable("dbo.CloudQueues");
            DropTable("dbo.WorkerRoles");
            DropTable("dbo.Bindings");
            DropTable("dbo.Sites");
            DropTable("dbo.RoleLogs");
            DropTable("dbo.LocalResouces");
            DropTable("dbo.InputEndpoints");
            DropTable("dbo.ConfigurationSettings");
            DropTable("dbo.WebRoles");
            DropTable("dbo.CloudServices");
            DropTable("dbo.CloudBlobs");
            DropTable("dbo.BlobDirectories");
            DropTable("dbo.CloudBlobContainers");
        }
    }
}
