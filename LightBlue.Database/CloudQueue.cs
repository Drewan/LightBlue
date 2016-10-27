using System;
using System.Collections.Generic;

namespace LightBlue.Database
{
    public class CloudQueue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public List<CloudQueueMessage> Messages { get; set; }
    }
}