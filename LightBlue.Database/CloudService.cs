using System.Collections.Generic;

namespace LightBlue.Database
{
    public class CloudService
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ServiceDefinition { get; set; }
        public string ServiceConfiguration { get; set; }
        public List<CloudRole> Roles { get; set; }

        public CloudService()
        {
            Roles = new List<CloudRole>();
        }
    }
}