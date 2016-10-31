namespace LightBlue.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CloudBlobContainers", newName: "BlobContainers");
            RenameTable(name: "dbo.CloudBlobs", newName: "Blobs");
            RenameTable(name: "dbo.LocalResouces", newName: "LocalResources");
            RenameTable(name: "dbo.CloudQueues", newName: "Queues");
            RenameTable(name: "dbo.CloudQueueMessages", newName: "QueueMessages");
            DropForeignKey("dbo.BlobDirectories", "CloudBlobContainerId", "dbo.CloudBlobContainers");
            DropIndex("dbo.BlobDirectories", new[] { "CloudBlobContainerId" });
            RenameColumn(table: "dbo.QueueMessages", name: "CloudQueue_Id", newName: "Queue_Id");
            RenameIndex(table: "dbo.QueueMessages", name: "IX_CloudQueue_Id", newName: "IX_Queue_Id");
            AddColumn("dbo.BlobDirectories", "BlobContainer_Id", c => c.Int());
            CreateIndex("dbo.BlobDirectories", "BlobContainer_Id");
            AddForeignKey("dbo.BlobDirectories", "BlobContainer_Id", "dbo.BlobContainers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlobDirectories", "BlobContainer_Id", "dbo.BlobContainers");
            DropIndex("dbo.BlobDirectories", new[] { "BlobContainer_Id" });
            DropColumn("dbo.BlobDirectories", "BlobContainer_Id");
            RenameIndex(table: "dbo.QueueMessages", name: "IX_Queue_Id", newName: "IX_CloudQueue_Id");
            RenameColumn(table: "dbo.QueueMessages", name: "Queue_Id", newName: "CloudQueue_Id");
            CreateIndex("dbo.BlobDirectories", "CloudBlobContainerId");
            AddForeignKey("dbo.BlobDirectories", "CloudBlobContainerId", "dbo.CloudBlobContainers", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.QueueMessages", newName: "CloudQueueMessages");
            RenameTable(name: "dbo.Queues", newName: "CloudQueues");
            RenameTable(name: "dbo.LocalResources", newName: "LocalResouces");
            RenameTable(name: "dbo.Blobs", newName: "CloudBlobs");
            RenameTable(name: "dbo.BlobContainers", newName: "CloudBlobContainers");
        }
    }
}
