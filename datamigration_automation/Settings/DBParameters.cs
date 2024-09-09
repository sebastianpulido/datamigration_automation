using System;
using Microsoft.Extensions.Configuration;

namespace datamigration_automation.Settings;

public class DBParameters : Parameters
{
    public string ConnectionString_Raw { get; set; }
    public string ConnectionString_Curate { get; set; }

    public DBParameters()
    {
        ConnectionString_Raw = Configuration.GetSection("DBParameters:connectionString_Raw").Value!;
        ConnectionString_Curate = Configuration.GetSection("DBParameters:connectionString_Curate").Value!;
    }
}
