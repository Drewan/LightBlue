using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LightBlue.Database
{
    public class ServiceDefinitionParser
    {
        public static ServiceDefinition Parse(string filepath)
        {
            using (var fs = File.OpenRead(filepath))
            {
                var serializer = new XmlSerializer(typeof(ServiceDefinition));
                return (ServiceDefinition)serializer.Deserialize(fs);
            }
        }

        [XmlRoot(ElementName = "Setting", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class Setting
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "ConfigurationSettings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class ConfigurationSettings
        {
            [XmlElement(ElementName = "Setting", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public List<Setting> Setting { get; set; }
        }

        [XmlRoot(ElementName = "WorkerRole", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class WorkerRole
        {
            [XmlElement(ElementName = "Imports", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public string Imports { get; set; }
            [XmlElement(ElementName = "ConfigurationSettings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public ConfigurationSettings ConfigurationSettings { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "vmsize")]
            public string Vmsize { get; set; }
        }

        [XmlRoot(ElementName = "Binding", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class Binding
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "endpointName")]
            public string EndpointName { get; set; }
        }

        [XmlRoot(ElementName = "Bindings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class Bindings
        {
            [XmlElement(ElementName = "Binding", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public Binding Binding { get; set; }
        }

        [XmlRoot(ElementName = "Site", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class Site
        {
            [XmlElement(ElementName = "Bindings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public Bindings Bindings { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "Sites", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class Sites
        {
            [XmlElement(ElementName = "Site", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public Site Site { get; set; }
        }

        [XmlRoot(ElementName = "InputEndpoint", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class InputEndpoint
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "protocol")]
            public string Protocol { get; set; }
            [XmlAttribute(AttributeName = "port")]
            public string Port { get; set; }
        }

        [XmlRoot(ElementName = "Endpoints", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class Endpoints
        {
            [XmlElement(ElementName = "InputEndpoint", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public InputEndpoint InputEndpoint { get; set; }
        }

        [XmlRoot(ElementName = "LocalStorage", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class LocalStorage
        {
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "cleanOnRoleRecycle")]
            public string CleanOnRoleRecycle { get; set; }
            [XmlAttribute(AttributeName = "sizeInMB")]
            public string SizeInMB { get; set; }
        }

        [XmlRoot(ElementName = "LocalResources", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class LocalResources
        {
            [XmlElement(ElementName = "LocalStorage", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public LocalStorage LocalStorage { get; set; }
        }

        [XmlRoot(ElementName = "WebRole", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class WebRole
        {
            [XmlElement(ElementName = "Sites", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public Sites Sites { get; set; }
            [XmlElement(ElementName = "Endpoints", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public Endpoints Endpoints { get; set; }
            [XmlElement(ElementName = "Imports", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public string Imports { get; set; }
            [XmlElement(ElementName = "LocalResources", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public LocalResources LocalResources { get; set; }
            [XmlElement(ElementName = "ConfigurationSettings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public ConfigurationSettings ConfigurationSettings { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "vmsize")]
            public string Vmsize { get; set; }
        }

        [XmlRoot(ElementName = "ServiceDefinition", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
        public class ServiceDefinition
        {
            [XmlElement(ElementName = "WorkerRole", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public List<WorkerRole> WorkerRole { get; set; }
            [XmlElement(ElementName = "WebRole", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceDefinition")]
            public WebRole WebRole { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "schemaVersion")]
            public string SchemaVersion { get; set; }
        }
    }

    public class XmlParser
    {
        public static ServiceConfiguration.Configuration Parse(string filepath)
        {
            using (var fs = File.OpenRead(filepath))
            {
                var serializer = new XmlSerializer(typeof(ServiceConfiguration.Configuration));
                return (ServiceConfiguration.Configuration)serializer.Deserialize(fs);
            }
        }

        public class ServiceConfiguration
        {
            [XmlRoot(ElementName = "ServiceConfiguration", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
            public class Configuration
            {
                [XmlElement(ElementName = "Role", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
                public List<Role> Role { get; set; }
                [XmlAttribute(AttributeName = "serviceName")]
                public string ServiceName { get; set; }
                [XmlAttribute(AttributeName = "xmlns")]
                public string Xmlns { get; set; }
                [XmlAttribute(AttributeName = "osFamily")]
                public string OsFamily { get; set; }
                [XmlAttribute(AttributeName = "osVersion")]
                public string OsVersion { get; set; }
                [XmlAttribute(AttributeName = "schemaVersion")]
                public string SchemaVersion { get; set; }
            }

            [XmlRoot(ElementName = "Role", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
            public class Role
            {
                [XmlElement(ElementName = "Instances", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
                public Instances Instances { get; set; }
                [XmlElement(ElementName = "ConfigurationSettings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
                public ConfigurationSettings ConfigurationSettings { get; set; }
                [XmlAttribute(AttributeName = "name")]
                public string Name { get; set; }
            }

            [XmlRoot(ElementName = "Instances", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
            public class Instances
            {
                [XmlAttribute(AttributeName = "count")]
                public string Count { get; set; }
            }

            [XmlRoot(ElementName = "Setting", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
            public class Setting
            {
                [XmlAttribute(AttributeName = "name")]
                public string Name { get; set; }
                [XmlAttribute(AttributeName = "value")]
                public string Value { get; set; }
            }

            [XmlRoot(ElementName = "ConfigurationSettings", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
            public class ConfigurationSettings
            {
                [XmlElement(ElementName = "Setting", Namespace = "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration")]
                public List<Setting> Setting { get; set; }
            }
        }
    }
}