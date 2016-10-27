using Topshelf;

namespace LightBlue.WorkerService
{
    class Program
    {
        static int Main(string[] args)
        {
            return (int)HostFactory.Run(x =>
            {
                var settings = new WorkerHostSettings();

                x.Service(hs => new WorkerHostService(settings.ToHostArgs()));

                x.AddCommandLineDefinition("a", a => { settings.Assembly = a; });
                x.AddCommandLineDefinition("n", n => { settings.RoleName = n; });
                x.AddCommandLineDefinition("t", t => { settings.ServiceTitle = t; });
                x.AddCommandLineDefinition("c", c => { settings.Configuration = c; });
                x.ApplyCommandLine();

                x.RunAsLocalSystem();
                x.SetDescription(settings.ServiceTitle);
                x.SetDisplayName(settings.ServiceTitle);
                x.SetServiceName(settings.ServiceTitle);
                x.StartManually();
            });
        }
    }
}
