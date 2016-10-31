using System.Collections.Generic;

namespace LightBlue.Database
{
    public class BlobDirectory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public List<Blob> Blobs { get; set; }

        public int CloudBlobContainerId { get; set; }
        public virtual BlobContainer BlobContainer { get; set; }
    }
}