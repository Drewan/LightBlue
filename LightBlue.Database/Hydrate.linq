<Query Kind="Program">
  <Connection>
    <ID>8372b8e5-4e44-40a5-b5e6-3f2b82c7a06b</ID>
    <Driver>EntityFrameworkDbContext</Driver>
    <CustomAssemblyPath>C:\Source\LightBlue\LightBlue.Database\bin\Debug\LightBlue.Database.dll</CustomAssemblyPath>
    <CustomTypeName>LightBlue.Database.LightBlueContext</CustomTypeName>
  </Connection>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
</Query>

void Main()
{
	var jsonText = File.ReadAllText(@"C:\Source\Mercury\Hosting\AllServices.json");
	var items = JsonConvert.DeserializeObject<Items>(jsonText);
	var roles = items.Roles.Where(x => x.EnabledOnStartup)
		.OrderByDescending(x => x.RoleName == "WebRole")
		.ToArray();
		
	Directory.SetCurrentDirectory(@"C:\Source\Mercury\Hosting");

	using (var db = new LightBlueContext())
	{
		foreach (var r in roles)
		{
			var cscfg = r.ConfigurationPath.EndsWith(".cscfg") 
				? r.ConfigurationPath 
				: Path.Combine(r.ConfigurationPath, "ServiceConfiguration.Local.cscfg");
			var csdef = Path.Combine(Path.GetDirectoryName(cscfg), "ServiceDefinition.csdef");

			var configurationSettings = XmlParser.Parse(cscfg).Role
				.Where(x => x.Name == r.RoleName)
				.SelectMany(x => x.ConfigurationSettings.Setting)
				.Select(x => new ConfigurationSetting { Name = x.Name, Value = x.Value })
				.ToList();
			
			var definition = ServiceDefinitionParser.Parse(csdef);

			var service = new CloudService
			{
				Name = r.Title,
				ServiceDefinition = csdef,
				ServiceConfiguration = cscfg
			};

			if (definition.WebRole != null && r.RoleName == "WebRole")
			{
				service.WebRoles.Add(new WebRole
				{
					Name = r.RoleName,
					IsEnabled = r.EnabledOnStartup,
					Assembly = r.Assembly,
					Instances = r.EnabledOnStartup ? 1 : 0,
					Sites = new List<Site>(),
					ConfigurationSettings = configurationSettings,
					LocalResouces = new List<LocalResouce>(),
					LogQueue = new List<RoleLog>(),
				});
			}
				
			var service = new CloudService
			{
				Name = r.Title,
				ServiceDefinition = csdef,
				ServiceConfiguration = cscfg,
				VmSize = definition.
				WebRoles = new List<WebRole>()  
				{
					new WebRole
					{
						Name = r.RoleName,
						Assembly = r.Assembly,
						Instances = 1,
						Sites = new List<Site>{ new Site { Name = definition.WebRole.Sites.Site.Name, Bindings = new Binding { Name = definition.WebRole.Sites.Site.Bindings.Binding.Name, EndPointName = } },
						ConfigurationSettings = settings,
						LocalResouces = new List<LocalResouce>(),
						LogQueue = new List<RoleLog>(),
					}
				},
				WorkerRoles = new List<WorkerRole>
				{
					new WorkerRole
					{
						Name = r.RoleName,
						Assembly = r.Assembly,
						Instances = 1,
						ConfigurationSettings = settings,
						LocalResouces = new List<LocalResouce>(),
						LogQueue = new List<RoleLog>(),
					}
				}
			};

//			db.CloudServices.Add(service);
//			db.SaveChanges();
		}


		// Display all Blogs from the database 
		var query = from b in db.CloudServices
					orderby b.Name
					select b;
		query.Dump();
	}
}

public static class ConfigurationLocator
{
	public static string LocateConfigurationFile(string configurationPath)
	{
		var path = RemoveTrailingDoubleQuote(configurationPath);

		if (File.Exists(path))
		{
			return path;
		}

		if (!Directory.Exists(path))
		{
			throw new ArgumentException("The configuration path does not exist. Specify the specific configuration file or the directory in which ServiceConfiguration.Local.cscfg is located.");
		}

		var configurationFile = Path.Combine(path, "ServiceConfiguration.Local.cscfg");
		if (!File.Exists(configurationFile))
		{
			throw new ArgumentException("ServiceConfiguration.Local.cscfg cannot be located in the configuration path.");
		}

		return configurationFile;
	}

	public static string LocateServiceDefinition(string configurationPath)
	{
		var path = RemoveTrailingDoubleQuote(configurationPath);

		var directoryPath = File.Exists(path)
			? Path.GetDirectoryName(path) ?? ""
			: path;

		if (!Directory.Exists(directoryPath))
		{
			throw new ArgumentException("The configuration path does not exist. Specify the specific configuration file or the directory in which ServiceConfiguration.Local.cscfg is located.");
		}

		var serviceDefinitionPath = Path.Combine(directoryPath, "ServiceDefinition.csdef");

		if (!File.Exists(serviceDefinitionPath))
		{
			throw new ArgumentException("Cannot locate the ServiceDefinition.csdef file. This must be in the configuration directory. If a cscfg file is specified ServiceDefinition.csdef must be in the same directory as this file.");
		}

		return serviceDefinitionPath;
	}

	private static string RemoveTrailingDoubleQuote(string configurationPath)
	{
		return configurationPath.EndsWith("\"")
			? configurationPath.Substring(0, configurationPath.Length - 1)
			: configurationPath;
	}

	public class Site
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<Binding> Bindings { get; set; }
	}

	public class Binding
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string EndPointName { get; set; }
	}

	public class InputEndpoint
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Protocol { get; set; }
		public int Port { get; set; }
	}
}


public class Items
{
	public HostSettings[] Roles { get; set; }
}

public class HostSettings
{
	public bool EnabledOnStartup { get; set; }
	public string Title { get; set; }
	public string RoleName { get; set; }
	public string Assembly { get; set; }
	public string ConfigurationPath { get; set; }
	public string Port { get; set; }
	public bool UseSsl { get; set; }
	public string Hostname { get; set; }

	public string GetTitle()
	{
		if (RoleName != "WebRole")
			return string.Format("Mercury{0}", Title);

		return string.Format("Mercury{0}{1}", Title, RoleName);
	}

	public string ToAbsoluteAssemblyPath(string mercuryRoot)
	{
		return mercuryRoot + "\\" + Assembly.Replace("..", string.Empty);
	}

	public string ToAbsoluteConfigurationPath(string mercuryRoot)
	{
		return mercuryRoot + "\\" + ConfigurationPath.Replace("..", string.Empty);
	}
}

