using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using IISExpressBootstrapper;
using LightBlue.Host.Stub;
using LightBlue.Infrastructure;
using LightBlue.Standalone;
using LightBlue.WebHost;
using Topshelf;
using TraceLevel = IISExpressBootstrapper.TraceLevel;

namespace LightBlue.WebService
{
    public class WebHostService : ServiceControl
    {
        private readonly WebHostArgs _hostArgs;
        private HostStub _stub;
        private IISExpressHost _iis;

        public WebHostService(WebHostArgs hostArgs)
        {
            _hostArgs = hostArgs;
        }

        public bool Start(HostControl hostControl)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    using (var webConfig = WebConfiguration.Load(_hostArgs.Assembly))
                    {
                        webConfig.RemoveTraceListener("Microsoft.WindowsAzure");
                    }

                    var applicationhost = Resources.ApplicationHostTemplate
                        .Replace("__SITEPATH__", _hostArgs.SiteDirectory)
                        .Replace("__PROTOCOL__", _hostArgs.UseSsl ? "https" : "http")
                        .Replace("__PORT__", _hostArgs.Port.ToString(CultureInfo.InvariantCulture))
                        .Replace("__HOSTNAME__", _hostArgs.Hostname);

                    var directory = new DirectoryInfo(StandaloneEnvironment.LightBlueDataDirectory + "//IISExpress");
                    if (!directory.Exists)
                        directory.Create();

                    var applicationHostFile = new FileInfo(string.Format(@"{0}\{1}applicationhost.config", 
                        directory.FullName, 
                        _hostArgs.Hostname));
                    if (applicationHostFile.Exists)
                        applicationHostFile.Delete();

                    File.WriteAllText(applicationHostFile.FullName, applicationhost);

                    var configuration = new ConfigFileParameters
                    {
                        ConfigFile = applicationHostFile.FullName,
                        SiteId = "1",
                        SiteName = "LightBlue",
                        Systray = true,
                        TraceLevel = TraceLevel.Info
                    };

                    _iis = IISExpressHost.Start(configuration,
                        _hostArgs.ToEnvironmentVariables(), 
                        output: s => Trace.WriteLine(s));
                }
                catch (Exception)
                {
                    hostControl.Stop();

                    throw;
                }
            });

            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    var runner = new HostRunner();
                    runner.Run(_hostArgs.Assembly,
                        _hostArgs.ConfigurationPath,
                        _hostArgs.ServiceDefinitionPath,
                        _hostArgs.RoleName,
                        _hostArgs.UseHostedStorage);
                }
                catch (Exception)
                {
                    hostControl.Stop();

                    throw;
                }
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _stub.RequestShutdown();

            return true;
        }
    }
}