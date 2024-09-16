using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Utilities;

static class QueryLibrary
{
    public static readonly string SQL_GET_TABLE_SCHEMA = "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =";
}
