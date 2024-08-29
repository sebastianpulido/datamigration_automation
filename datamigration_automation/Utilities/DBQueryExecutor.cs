using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace datamigration_automation.Utilities;

internal class DBQueryExecutor(string sqlQuery, string connectionString)
{
    private readonly string _sqlQuery = sqlQuery;

    private readonly string _connectionString = connectionString;

    public DataTable FetchData()
    {
        DataTable dataTable = new();
        DBConnectionHelper dBConnection = new(_connectionString);
        
        try
        {
            using SqlConnection sqlConnection = dBConnection.GetConnection();
            using SqlCommand sqlCommand = new(_sqlQuery, sqlConnection);
            using SqlDataAdapter dataAdapter = new(sqlCommand);
            dataAdapter.Fill(dataTable);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        
        return dataTable;
    }
}
