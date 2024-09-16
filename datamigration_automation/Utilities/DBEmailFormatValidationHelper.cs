using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace datamigration_automation.Utilities;

internal class DBEmailFormatValidationHelper(string connectionString1, string connectionString2)
{

    public string ValidateEmailFormats(string table1, string schema1, string table2, string schema2)
    {
        var report = new StringBuilder();

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        // Retrieve and validate emails from the first table
        var emailsTable1 = GetEmailAddresses(table1, schema1, connectionString1);
        foreach (var email in emailsTable1)
        {
            if (!Regex.IsMatch(email, emailPattern))
            {
                report.AppendLine($"Invalid email format in {schema1}.{table1}: {email}");
            }
        }

        // Retrieve and validate emails from the second table
        var emailsTable2 = GetEmailAddresses(table2, schema2, connectionString2);
        foreach (var email in emailsTable2)
        {
            if (!Regex.IsMatch(email, emailPattern))
            {
                report.AppendLine($"Invalid email format in {schema2}.{table2}: {email}");
            }
        }

        return report.ToString();
    }

    private IEnumerable<string> GetEmailAddresses(string tableName, string schemaName, string connectionString)
    {
        var emails = new List<string>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = $"SELECT EmailColumn FROM {schemaName}.{tableName}";

            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emails.Add(reader["EmailColumn"].ToString());
                    }
                }
            }
        }

        return emails;
    }
}
