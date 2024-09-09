using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace datamigration_automation.Settings;

public class Parameters
{
    public IConfigurationRoot Configuration { get; }

    public Parameters() 
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }
}
