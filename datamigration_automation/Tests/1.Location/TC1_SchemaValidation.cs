using System;
using datamigration_automation.Utilities;
using System.Data;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using datamigration_automation.Main;

namespace datamigration_automation.Tests._1.Location;

[TestFixture]
[Parallelizable]
public class TC1_SchemaValidation : BaseTestCase
{
    

    private readonly static TestCaseData[] TestCaseData = {
        new("table_name1", "table_name2"),
        new("table_name3", "table_name4"),
    };

    public TC1_SchemaValidation()
    {
        TestCaseID = "01";
        TestCaseName = "Name";
        TestCaseDescription = "Description";
        Schema1TableName = "";
        Schema2TableName = "";
    }

    [Test]
    [Category("Integration")]
    [TestCaseSource("TestCaseData")]
    public void TC1_ValidateTableSchemas(string tableName1, string tableName2)
    {
        DBConnectionHelper dBConnectionHelper = new(DB._dbParameters.ConnectionString_Raw);
        Console.WriteLine($"connection string: {DB._dbParameters.ConnectionString_Raw}");

        DataTable table = new DBQueryExecutor("select * from dbo.Territory;", DB._dbParameters.ConnectionString_Raw).FetchData();
        Console.WriteLine("rows: " + table.Rows.Count);

        DBSchemaValidationHelper dBSchemaValidationHelper = new(DB._dbParameters.ConnectionString_Raw, DB._dbParameters.ConnectionString_Curate);
        var schemaValidation = dBSchemaValidationHelper.CompareSchemas("dbo.Territory", "dbo.Territory");
        Console.WriteLine($"schemaValidation: {schemaValidation}");
    }
}
