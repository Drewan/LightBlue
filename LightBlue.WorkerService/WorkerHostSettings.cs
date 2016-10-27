using LightBlue.Host;

namespace LightBlue.WorkerService
{
    public class WorkerHostSettings
    {
        public string Assembly { get; set; }
        public string RoleName { get; set; }
        public string ServiceTitle { get; set; }
        public string Configuration { get; set; }

        public HostArgs ToHostArgs()
        {
            return HostArgs.ParseArgs(new[]
            {
                "-a:" + Assembly,
                "-n:" + RoleName,
                "-t:" + ServiceTitle,
                "-c:" + Configuration
            });
        }
    }
}