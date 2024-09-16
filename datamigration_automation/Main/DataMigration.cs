using datamigration_automation.Settings;
using datamigration_automation.Utilities;
using System;

namespace datamigration_automation.Main;

public class DataMigration
{
    public readonly DBParameters _dbParameters;

    // Instance of the singleton
    private static DataMigration? _instance;

    // Lock object for thread safety
    private static readonly object _lock = new();

    public static DataMigration Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataMigration();
                    }
                }
            }
            return _instance;
        }
    }

    public DataMigration()
    {
        _dbParameters = new DBParameters();
    }

    public void GenerateReport()
    {
        TestReport testReport = new();
        testReport.GenerateValidationReport();
    }
}