using System;

namespace LightBlue.Database
{
    public class QueueMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int CloudQueueId { get; set; }
        public virtual Queue Queue { get; set; }
    }
}