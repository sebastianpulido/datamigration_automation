using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Utilities;

internal class DBTableSchemaValidationHelper(string connectionString1, string connectionString2)
{
    public string CompareTables(string table1, string table2)
    {
        var table1Schema = GetTableSchema(table1, connectionString1);
        var table2Schema = GetTableSchema(table2, connectionString2);

        var report = new StringBuilder();
        report.AppendLine($"Comparison between '{table1}' and '{table2}' = ");

        // Compare columns
        foreach (var column1 in table1Schema)
        {
            if (table2Schema.ContainsKey(column1.Key))
            {
                var column2 = table2Schema[column1.Key];
                if (column1.Value != column2)
                {
                    report.AppendLine($"Column '{column1.Key}' has different types: {table1}({column1.Value}) vs {table2}({column2})");
                }
            }
            else
            {
                report.AppendLine($"Column '{column1.Key}' exists in '{table1}' but not in '{table2}'");
            }
        }

        // Check columns in table2 that are not in table1
        foreach (var column2 in table2Schema)
        {
            if (!table1Schema.ContainsKey(column2.Key))
            {
                report.AppendLine($"Column '{column2.Key}' exists in '{table2}' but not in '{table1}'");
            }
        }

        return report.ToString();
    }

    private Dictionary<string, string> GetTableSchema(string tableName, string connectionString)
    {
        var schema = new Dictionary<string, string>();
        string query = $"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnName = reader["COLUMN_NAME"].ToString();
                        var dataType = reader["DATA_TYPE"].ToString();
                        schema[columnName] = dataType;
                    }
                }
            }
        }

        return schema;
    }
}
