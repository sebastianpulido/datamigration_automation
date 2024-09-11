using datamigration_automation.Main;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Tests;

[TestFixture]
[Parallelizable]
public class BaseTestCase
{
    protected DataMigration DB { get; } = new DataMigration();


    public bool TestCaseResult = false;

    public string TestCaseID { get; set; } = string.Empty;

    public string TestCaseName { get; set; } = string.Empty;

    public string TestCaseDescription { get; set; } = string.Empty;

    public string Schema1TableName { get; set; } = string.Empty;

    public string Schema2TableName { get; set; } = string.Empty;

    [SetUp]
    public void OnBaseStart()
    {
    }

    [TearDown]
    public void OnBaseFinish()
    {
    }
}