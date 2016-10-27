using LightBlue.Standalone;
using Xunit;

namespace LightBlue.Tests.Web
{
    public class WebConfigurationTests
    {
        [Fact]
        public void CanLoadWebConfiguration()
        {
            var settings = new StandaloneAzureSettings(new StandaloneConfiguration
            {
                ConfigurationPath = "ServiceConfiguration.Local.cscfg",
                RoleName = "TestWorkerRole"
            });

            Assert.Equal("Running locally", settings["RandomSetting"]);
        }
    }
}