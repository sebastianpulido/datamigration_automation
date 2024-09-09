using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Tests;

internal class BaseTestCase
{
    public bool TestCaseResult = false;

    public string TestCaseID { get; set; } = string.Empty;

    public string TestCaseName { get; set; } = string.Empty;

    public string TestCaseDescription { get; set; } = string.Empty;

    public string Schema1TableName { get; set; } = string.Empty;

    public string Schema2TableName { get; set; } = string.Empty;

}