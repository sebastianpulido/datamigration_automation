using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datamigration_automation.Utilities;

internal class DBTableDataValidationHelper(string connectionString)
{
    // Validate the number of rows in a table
    public int ValidateNumberOfRows(string tableName)
    {
        int rowCount = 0;

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = $"SELECT COUNT(*) FROM {tableName}";

            using (var command = new SqlCommand(query, connection))
            {
                rowCount = (int)command.ExecuteScalar();
            }
        }

        return rowCount;
    }

    // Validate the number of columns in a table against a provided list
    public List<string> ValidateNumberOfColumns(string tableName, List<string> expectedColumns, string connectionString)
    {
        var actualColumns = new List<string>();
        var missingColumns = new List<string>();
        var extraColumns = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = $@"
                    SELECT COLUMN_NAME 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = '{tableName}'";

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actualColumns.Add(reader["COLUMN_NAME"].ToString());
                    }
                }
            }
        }

        // Determine missing and extra columns
        missingColumns = expectedColumns.Except(actualColumns).ToList();
        extraColumns = actualColumns.Except(expectedColumns).ToList();

        // Print missing and extra columns
        var report = new StringBuilder();
        if (missingColumns.Any())
        {
            report.AppendLine("Missing Columns:");
            foreach (var column in missingColumns)
            {
                report.AppendLine(column);
            }
        }
        else
        {
            report.AppendLine("No missing columns.");
        }

        if (extraColumns.Any())
        {
            report.AppendLine("Extra Columns:");
            foreach (var column in extraColumns)
            {
                report.AppendLine(column);
            }
        }
        else
        {
            report.AppendLine("No extra columns.");
        }

        Console.WriteLine(report.ToString());

        // Return the list of missing columns
        return missingColumns;
    }


    // Validate uniqueness of specified columns
    public void ValidateUniqueness(string tableName, List<string> columnsToValidate, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var report = new StringBuilder();

            foreach (var column in columnsToValidate)
            {
                var query = $@"
                        SELECT {column}, COUNT(*)
                        FROM {tableName}
                        GROUP BY {column}
                        HAVING COUNT(*) > 1";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            report.AppendLine($"Non-unique values found in column '{column}':");
                            while (reader.Read())
                            {
                                var value = reader[column].ToString();
                                var count = reader.GetInt32(1);
                                report.AppendLine($"Value '{value}' appears {count} times.");
                            }
                        }
                    }
                }
            }

            if (report.Length == 0)
            {
                report.AppendLine("All specified columns are unique.");
            }

            Console.WriteLine(report.ToString());
        }
    }

    // Validate that numeric values fall within acceptable ranges
    public void ValidateValueRanges(string tableName, Dictionary<string, (int Min, int Max)> columnRanges, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var report = new StringBuilder();

            foreach (var columnRange in columnRanges)
            {
                var columnName = columnRange.Key;
                var minValue = columnRange.Value.Min;
                var maxValue = columnRange.Value.Max;

                var query = $@"
                        SELECT {columnName}
                        FROM {tableName}
                        WHERE {columnName} < @MinValue OR {columnName} > @MaxValue";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MinValue", minValue);
                    command.Parameters.AddWithValue("@MaxValue", maxValue);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            report.AppendLine($"Values in column '{columnName}' outside the range {minValue} - {maxValue}:");
                            while (reader.Read())
                            {
                                var value = reader[columnName];
                                report.AppendLine($"Value '{value}' is out of range.");
                            }
                        }
                    }
                }
            }

            if (report.Length == 0)
            {
                report.AppendLine("All values are within the acceptable ranges.");
            }

            Console.WriteLine(report.ToString());
        }
    }

    // Validate that values in specified columns are present in a master table
    public void ValidateValuesAgainstMaster(string tableName, List<string> columnsToCheck, string masterTableName, string masterColumnName, string connectionString)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var report = new StringBuilder();

            foreach (var column in columnsToCheck)
            {
                var query = $@"
                        SELECT DISTINCT {column}
                        FROM {tableName}
                        WHERE {column} NOT IN (
                            SELECT {masterColumnName}
                            FROM {masterTableName}
                        )";

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            report.AppendLine($"Values in column '{column}' not found in master table '{masterTableName}':");
                            while (reader.Read())
                            {
                                var value = reader[column].ToString();
                                report.AppendLine($"Value '{value}' is missing from the master table.");
                            }
                        }
                    }
                }
            }

            if (report.Length == 0)
            {
                report.AppendLine("All values are present in the master table.");
            }

            Console.WriteLine(report.ToString());
        }
    }
}
