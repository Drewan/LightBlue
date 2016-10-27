﻿using Formo;
using LightBlue.WebHost;
using Topshelf;

namespace LightBlue.WebService
{
    class Program
    {
        static int Main(string[] args)
        {
           return (int)HostFactory.Run(x =>
           {
               dynamic configuration = new Configuration();
               WebHostSettings settings = configuration.Bind<WebHostSettings>();

               x.Service<WebHostService>(service =>
               {
                   service.ConstructUsing(s =>
                   {
                       var hostArgs = WebHostArgs.ParseArgs(new[]
                       {
                           "-a:" + settings.Assembly,
                           "-p:" + settings.Port,
                           "-n:" + settings.RoleName,
                           "-t:" + settings.ServiceTitle,
                           "-c:" + settings.Configuration,
                           "-s:" + settings.UseSSL,
                           "-h:" + settings.Host
                       });
                       return new WebHostService(hostArgs);
                   });
                   service.WhenStarted((s, h) => s.Start(h));
                   service.WhenStopped((s, h) => s.Stop(h));
               });

               x.RunAs("CASHCONVERTERS\\drewan.obrien", "exGfbm7c");
               x.SetDescription(settings.ServiceTitle);
               x.SetDisplayName(settings.ServiceTitle);
               x.SetServiceName(settings.ServiceTitle);
               x.StartManually();
           });
        }
    }
}
