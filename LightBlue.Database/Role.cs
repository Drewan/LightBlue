using System.Collections.Generic;

namespace LightBlue.Database
{
    public abstract class Role
    {
        public int Id { get; set; }
        public string Assembly { get; set; }
        public string Name { get; set; }
        public int Instances { get; set; }
        public bool IsEnabled { get; set; }
        public virtual List<InputEndpoint> Endpoints { get; set; }
        public virtual List<LocalResource> LocalResouces { get; set; }
        public virtual List<ConfigurationSetting> ConfigurationSettings { get; set; }
        public virtual List<RoleLog> LogQueue { get; set; }

        public int CloudServiceId { get; set; }
        public virtual CloudService CloudService { get; set; }

        protected Role()
        {
            LocalResouces = new List<LocalResource>();
            ConfigurationSettings = new List<ConfigurationSetting>();
            LogQueue = new List<RoleLog>();
        }
    }
}