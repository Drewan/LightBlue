using System;
using System.Collections.Generic;

namespace LightBlue.Database
{
    public class Queue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public List<QueueMessage> Messages { get; set; }
    }
}