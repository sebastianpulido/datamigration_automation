using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Utilities;

internal class DBSchemaValidationHelper(string connectionString1, string connectionString2)
{
    private readonly string _connectionString1 = connectionString1;
    private readonly string _connectionString2 = connectionString2;

    public bool CompareSchemas(string tableName1, string tableName2)
    {
        var schema1 = GetTableSchema(_connectionString1, tableName1);
        var schema2 = GetTableSchema(_connectionString2, tableName2);

        return CompareSchemas(schema1, schema2);
    }

    private DataTable GetTableSchema(string connectionString, string tableName)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var schemaTable = connection.GetSchema("Columns", new[] { null, null, tableName });
            return schemaTable;
        }
    }

    private bool CompareSchemas(DataTable schema1, DataTable schema2)
    {
        var columns1 = schema1.AsEnumerable()
            .Select(row => new
            {
                ColumnName = row.Field<string>("COLUMN_NAME"),
                DataType = row.Field<string>("DATA_TYPE")
            })
            .ToList();

        var columns2 = schema2.AsEnumerable()
            .Select(row => new
            {
                ColumnName = row.Field<string>("COLUMN_NAME"),
                DataType = row.Field<string>("DATA_TYPE")
            })
            .ToList();

        if (columns1.Count != columns2.Count)
        {
            Console.WriteLine("Column count mismatch.");
            return false;
        }

        var missingInSchema2 = columns1.Except(columns2).ToList();
        var missingInSchema1 = columns2.Except(columns1).ToList();

        if (missingInSchema2.Any() || missingInSchema1.Any())
        {
            Console.WriteLine("Schema mismatch detected:");
            Console.WriteLine("Missing in second schema:");
            foreach (var column in missingInSchema2)
            {
                Console.WriteLine($"- {column.ColumnName} ({column.DataType})");
            }

            Console.WriteLine("Missing in first schema:");
            foreach (var column in missingInSchema1)
            {
                Console.WriteLine($"- {column.ColumnName} ({column.DataType})");
            }

            return false;
        }

        Console.WriteLine("Schemas are identical.");
        return true;
    }
}
