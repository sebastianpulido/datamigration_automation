using datamigration_automation.Settings;
using datamigration_automation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Main;

public class DataMigration
{
    public readonly DBParameters _dbParameters;

    private DBConnectionHelper dBConnectionHelper_source;

    private DBConnectionHelper dBConnectionHelper_target;

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
        dBConnectionHelper_source = GetDataBaseConnection(_dbParameters.ConnectionString_Raw)!;
        dBConnectionHelper_target = GetDataBaseConnection(_dbParameters.ConnectionString_Curate)!;
    }

    public void StartValidation()
    {
        TestReport testReport = new();
        testReport.GenerateValidationReport();
    }

    internal static DBConnectionHelper? GetDataBaseConnection(string connectionString)
    {
        try
        {
            return new DBConnectionHelper(connectionString);    
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }

    

    /*public static void Main(string[] args)
    {
        DataMigration main = new();
        DBConnectionHelper dBConnectionHelper = new(main._dbParameters.ConnectionString_Raw);
        Console.WriteLine($"connection: {main._dbParameters.ConnectionString_Raw}");

        DataTable table = new DBQueryExecutor("select * from dbo.Territory;", main._dbParameters.ConnectionString_Raw).FetchData();
        Console.WriteLine("rows: " + table.Rows.Count);

        DBSchemaValidationHelper dBSchemaValidationHelper = new(main._dbParameters.ConnectionString_Raw, main._dbParameters.ConnectionString_Curate);
        var schemaValidation = dBSchemaValidationHelper.CompareSchemas("dbo.Territory", "dbo.Territory");
        Console.WriteLine($"schemaValidation: {schemaValidation}");
    }*/
}
