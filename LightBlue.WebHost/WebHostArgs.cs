﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using LightBlue.Infrastructure;
using NDesk.Options;

namespace LightBlue.WebHost
{
    public class WebHostArgs
    {
        public string Assembly { get; private set; }
        public int Port { get; private set; }
        public string RoleName { get; private set; }
        public string Title { get; private set; }
        public string ConfigurationPath { get; private set; }
        public string ServiceDefinitionPath { get; private set; }
        public bool UseSsl { get; private set; }
        public string Hostname { get; private set; }
        public bool UseHostedStorage { get; private set; }
        public bool AllowSilentFail { get; private set; }
        public string IisExpressTemplate { get; private set; }
        public bool Use64Bit { get; private set; }

        public string SiteDirectory
        {
            get { return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly) ?? "", "..")); }
        }

        public string SiteBinDirectory
        {
            get { return Path.GetDirectoryName(Assembly) ?? ""; }
        }

        public static WebHostArgs ParseArgs(IEnumerable<string> args)
        {
            string assembly = null;
            int? port = null;
            string roleName = null;
            string title = null;
            string configurationPath = null;
            bool? useSsl = null;
            var hostname = "localhost";
            var useHostedStorage = false;
            var allowSilentFail = false;
            string iisExpressTemplate = null;
            var displayHelp = false;
            bool use64Bit = false;

            var options = new OptionSet
            {
                {
                    "a|assembly=",
                    "The path to the primary {ASSEMBLY} for the web role.",
                    v => assembly = v
                },
                {
                    "p|port=",
                    "The {PORT} on which the website should be available. Must be specified if useSsl is specified.",
                    (int v) => port = v
                },
                {
                    "n|roleName=",
                    "The {NAME} of the role as defined in the configuration file.",
                    v => roleName = v
                },
                {
                    "t|serviceTitle=",
                    "Optional {TITLE} for the role window. Defaults to role name if not specified.",
                    v => title = v
                },
                {
                    "c|configurationPath=",
                    "The {PATH} to the configuration file. Either the directory containing ServiceConfiguration.Local.cscfg or the path to a specific alternate .cscfg file.",
                    v => configurationPath = v
                },
                {
                    "s|useSsl=",
                    "Indicates whether the site should be started with SSL. Defaults to false. Must be specified in port is specified.",
                    (bool v) => useSsl = v
                },
                {
                    "h|hostname=",
                    "The {HOSTNAME} the site should be started with. Defaults to localhost",
                    v => hostname = v
                },
                {
                    "useHostedStorage",
                    "Use hosted storage (Emulator/Actual Azure) inside the LightBlue host.",
                    v => useHostedStorage = true
                },
                {
                    "allowSilentFail",
                    "Allow the host to fail silently instead of throwing an exception when the hosted process exits. Applies only to the background process not the website.",
                    v => allowSilentFail = true
                },
                {
                    "iet|iisExpressTemplate=",
                    "Path to an alternate {TEMPLATE} for IIS Express. If not specified the inbuilt default is used.",
                    v => iisExpressTemplate = v
                },
                {
                    "help",
                    "Show this message and exit.",
                    v => displayHelp = true
                },
                {
                    "u|use64bit=",
                    "Indicates whether to use IISExpress 64 bit. Defaults to false.",
                    (bool v) => use64Bit = v
                }
            };

            try
            {
                options.Parse(args);
            }
            catch (OptionException e)
            {
                DisplayErrorMessage(e.Message);
                return null;
            }

            if (displayHelp)
            {
                DisplayHelp(options);
                return null;
            }

            if (string.IsNullOrWhiteSpace(assembly))
            {
                DisplayErrorMessage("Host requires an assembly to run.");
                return null;
            }
            if (string.IsNullOrWhiteSpace(roleName))
            {
                DisplayErrorMessage("Role Name must be specified.");
                return null;
            }
            if (string.IsNullOrWhiteSpace(configurationPath))
            {
                DisplayErrorMessage("Configuration path must be specified.");
                return null;
            }
            if (string.IsNullOrWhiteSpace(hostname))
            {
                DisplayErrorMessage(
                    "The hostname cannot be blank. Do not specify this option if you wish to use the default (localhost).");
                return null;
            }

            var roleAssemblyAbsolutePath = Path.IsPathRooted(assembly)
                ? assembly
                : Path.Combine(Environment.CurrentDirectory, assembly);

            if (!File.Exists(roleAssemblyAbsolutePath))
            {
                DisplayErrorMessage("The specified site assembly cannot be found.");
                return null;
            }

            if (port.HasValue != useSsl.HasValue)
            {
                DisplayErrorMessage("If either port or useSsl is specified both must be specified.");
                return null;
            }

            var serviceDefinitionPath = ConfigurationLocator.LocateServiceDefinition(configurationPath);

            if (!port.HasValue)
            {
                var endpoint = GetEndpoint(serviceDefinitionPath, roleName);
                if (endpoint == null)
                {
                    DisplayErrorMessage(
                        "Cannot locate endpoint in ServiceDefinition.csdef. Verify that an endpoint is defined.");
                    return null;
                }

                port = endpoint.Item1;
                useSsl = endpoint.Item2;
            }

            if (!string.IsNullOrWhiteSpace(iisExpressTemplate) && !File.Exists(iisExpressTemplate))
            {
                DisplayErrorMessage("The specified IIS Express template cannot be located.");
                return null;
            }

            if (use64Bit && (!Environment.Is64BitOperatingSystem && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ProgramW6432"))))
            {
                DisplayErrorMessage("Use64Bit flag cannot be true on a 32bit platform");
                return null;
            }

            return new WebHostArgs
            {
                Assembly = assembly,
                Port = port.Value,
                RoleName = roleName,
                Title = string.IsNullOrWhiteSpace(title)
                    ? roleName
                    : title,
                ConfigurationPath = ConfigurationLocator.LocateConfigurationFile(configurationPath),
                ServiceDefinitionPath = serviceDefinitionPath,
                UseSsl = useSsl.Value,
                Hostname = hostname,
                UseHostedStorage = useHostedStorage,
                AllowSilentFail = allowSilentFail,
                IisExpressTemplate = iisExpressTemplate,
                Use64Bit = use64Bit
            };
        }

        private static Tuple<int, bool> GetEndpoint(string serviceDefinitionPath, string roleName)
        {
            var xDocument = XDocument.Load(serviceDefinitionPath);

            XNamespace serviceDefinitionNamespace = xDocument.Root
                .Attributes()
                .Where(a => a.IsNamespaceDeclaration)
                .First(a => a.Value.Contains("ServiceDefinition"))
                .Value;

            var roleElement = xDocument.Root.Elements()
                .FirstOrDefault(e => e.Attribute("name").Value == roleName);

            if (roleElement == null)
            {
                return null;
            }

            var endpointsElement = roleElement.Descendants(serviceDefinitionNamespace + "Endpoints")
                .FirstOrDefault();

            if (endpointsElement == null)
            {
                return null;
            }

            var endpointElement = endpointsElement.Descendants(serviceDefinitionNamespace + "InputEndpoint")
                .FirstOrDefault();

            if (endpointElement == null)
            {
                return null;
            }

            var portAttribute = endpointElement.Attribute("port");
            var protocolAttribute = endpointElement.Attribute("protocol");
            if (portAttribute == null || protocolAttribute == null)
            {
                return null;
            }

            int port;

            if (!Int32.TryParse(portAttribute.Value, out port) || string.IsNullOrWhiteSpace(protocolAttribute.Value))
            {
                return null;
            }

            return Tuple.Create(port, protocolAttribute.Value.ToLowerInvariant() == "https");
        }

        private static void DisplayErrorMessage(string message)
        {
            Console.Write("LightBlue.WebHost: ");
            Console.WriteLine(message);
            Console.WriteLine("Try `LightBlue.WebHost --help' for more information.");
        }

        private static void DisplayHelp(OptionSet p)
        {
            Console.WriteLine("Usage: LightBlue.WebHost [OPTIONS]");
            Console.WriteLine("Host a LightBlue based Azure web role.");
            Console.WriteLine("IIS Express is used to host the web site.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

        public IDictionary<string, string> ToEnvironmentVariables()
        {
            var variables = new Dictionary<string, string>();
            variables.Add("LightBlueHost", "true");
            variables.Add("LightBlueConfigurationPath", ConfigurationPath);
            variables.Add("LightBlueServiceDefinitionPath", ServiceDefinitionPath);
            variables.Add("LightBlueRoleName", RoleName);
            variables.Add("LightBlueUseHostedStorage", UseHostedStorage.ToString());
            return variables;
        }
    }
}
