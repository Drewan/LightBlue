using System.Collections.Generic;

namespace LightBlue.Database
{
    public class CloudRole
    {
        public int Id { get; set; }
        public string Assembly { get; set; }
        public string Name { get; set; }
        public int Instances { get; set; }
        public bool IsEnabled { get; set; }
        public virtual List<LocalResouce> LocalResouces { get; set; }
        public virtual List<ConfigurationSetting> ConfigurationSettings { get; set; }
        public virtual List<RoleLog> LogQueue { get; set; }

        public int CloudServiceId { get; set; }
        public virtual CloudService CloudService { get; set; }

        public CloudRole()
        {
            LocalResouces = new List<LocalResouce>();
            ConfigurationSettings = new List<ConfigurationSetting>();
            LogQueue = new List<RoleLog>();
        }
    }
}