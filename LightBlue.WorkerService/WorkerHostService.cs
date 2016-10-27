using System;
using System.Threading;
using LightBlue.Host;
using Topshelf;

namespace LightBlue.WorkerService
{
    public class WorkerHostService : ServiceControl
    {
        private readonly HostArgs _hostArgs;
        //private HostStub _stub;

        public WorkerHostService(HostArgs hostArgs)
        {
            _hostArgs = hostArgs;
        }
         
        public bool Start(HostControl hostControl)
        {
            ThreadPool.QueueUserWorkItem(x =>
            {
                try
                {
                    //Infrastructure.ConfigurationManipulation.RemoveAzureTraceListenerFromConfiguration(_hostArgs.RoleConfigurationFile);

                    //_stub = WorkerHostFactory.Create(_hostArgs);
                    //_stub.Run(_hostArgs.Assembly,
                    //    _hostArgs.ConfigurationPath,
                    //    _hostArgs.ServiceDefinitionPath,
                    //    _hostArgs.RoleName,
                    //    _hostArgs.UseHostedStorage);
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
           // _stub.RequestShutdown();

            return true;
        }
    }
}