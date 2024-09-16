using System;
using datamigration_automation.Utilities;
using System.Data;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using datamigration_automation.Main;
using NUnit.Framework.Legacy;

namespace datamigration_automation.Tests._1.Location;

[TestFixture]
[Parallelizable]
public class TCs_1_Location : BaseTestCase
{
    private bool TestCaseResult = false;

    private readonly static TestCaseData[] TestCaseData = {
        //new("dbo.Territory", "dbo.sla"),
        new("kpmg_costcode", "kpmg_costcode"),
    };

    [Test]
    [Category("System")]
    [TestCaseSource("TestCaseData")]
    public void TC1_ValidateTableSchemas(string tableName1, string tableName2)
    {
        DBTableSchemaValidationHelper dBTableSchemaValidationHelper = new(DB._dbParameters.ConnectionString_Raw, DB._dbParameters.ConnectionString_Curate);
        var schemaValidation = dBTableSchemaValidationHelper.CompareTables(tableName1, tableName2);
        ClassicAssert.AreEqual($"Comparison between '{tableName1}' and '{tableName2}' = ", schemaValidation);
    }

    private readonly static TestCaseData[] TestCaseData2 = {
        //new("dbo.Territory", "dbo.sla"),
        new("dbo.kpmg_costcode", "dbo.kpmg_costcode"),
    };

    [Test]
    [Category("System")]
    [TestCaseSource("TestCaseData2")]
    public void TC1_ValidateNumberOfRows(string tableName1, string tableName2)
    {
        int rows1 = 0;
        int rows2 = 0;
        DBConnectionHelper dBConnectionHelper = new(DB._dbParameters.ConnectionString_Raw);
        Console.WriteLine($"connection string: {DB._dbParameters.ConnectionString_Raw}");

        DataTable table = new DBQueryExecutor($"select * from {tableName1};", DB._dbParameters.ConnectionString_Raw).FetchData();
        rows1 = table.Rows.Count;
        Console.WriteLine($"rows table {tableName1}: {rows1}");
        
        table = new DBQueryExecutor($"select * from {tableName2};", DB._dbParameters.ConnectionString_Curate).FetchData();
        rows2 = table.Rows.Count;
        Console.WriteLine($"rows table {tableName2}: {rows2}");

        ClassicAssert.AreEqual(rows1, rows2);
    }
}