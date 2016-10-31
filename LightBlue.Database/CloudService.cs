using System.Collections.Generic;

namespace LightBlue.Database
{
    public class CloudService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VmSize { get; set; }
        public string ServiceDefinition { get; set; }
        public string ServiceConfiguration { get; set; }

        public List<WebRole> WebRoles { get; set; }
        public List<WorkerRole> WorkerRoles { get; set; }


        public CloudService()
        {
            WebRoles = new List<WebRole>();
            WorkerRoles = new List<WorkerRole>();
        }
    }
}