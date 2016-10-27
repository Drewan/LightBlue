using System;

namespace LightBlue.Database
{
    public class CloudQueueMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int CloudQueueId { get; set; }
        public virtual CloudQueue CloudQueue { get; set; }
    }
}