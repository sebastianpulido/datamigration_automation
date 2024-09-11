using NUnit.Framework;
using System;

namespace datamigration_automation.Tests;

[SetUpFixture]
public class Setup
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        //TestData.LoadTestData();
    }
}