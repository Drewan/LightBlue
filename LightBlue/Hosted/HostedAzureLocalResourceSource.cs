﻿using Microsoft.WindowsAzure.ServiceRuntime;

namespace LightBlue.Hosted
{
    public class HostedAzureLocalResourceSource : IAzureLocalResourceSource
    {
        public IAzureLocalResource this[string index]
        {
            get { return new HostedAzureLocalResource(RoleEnvironment.GetLocalResource(index)); }
        }
    }
}