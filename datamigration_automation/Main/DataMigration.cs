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

    public DataMigration()
    {
        _dbParameters = new DBParameters();
    }

    public void StartValidation()
    {
        TestReport testReport = new();
        testReport.GenerateValidationReport();
    }

    /*public static void Main(string[] args)
    {
        DataMigration main = new();
        DBConnectionHelper dBConnectionHelper = new(main._dbParameters.ConnectionString_Raw);
        Console.WriteLine($"here: {main._dbParameters.ConnectionString_Raw}");

        DataTable table = new DBQueryExecutor("select * from dbo.Territory;", main._dbParameters.ConnectionString_Raw).FetchData();
        Console.WriteLine("rows: " + table.Rows.Count);

        DBSchemaValidationHelper dBSchemaValidationHelper = new(main._dbParameters.ConnectionString_Raw, main._dbParameters.ConnectionString_Curate);
        var schemaValidation = dBSchemaValidationHelper.CompareSchemas("dbo.Territory", "dbo.Territory");
        Console.WriteLine($"schemaValidation: {schemaValidation}");
    }*/
}
