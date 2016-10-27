using System;

namespace LightBlue.Database
{
    public class CloudBlob
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int Length { get; set; }
        
        public int BlobDirectoryId { get; set; }
        public virtual BlobDirectory BlobDirectory  { get; set; }
    }
}