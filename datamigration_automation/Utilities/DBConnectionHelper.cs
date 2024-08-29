using System;
using Microsoft.Data.SqlClient;

namespace datamigration_automation.Utilities;

internal class DBConnectionHelper(string connectionString) : IDisposable
{
    private SqlConnection? _sqlConnection;

    public SqlConnection GetConnection()
    {
        if (_sqlConnection == null)
        {
            _sqlConnection = new SqlConnection(connectionString);
            _sqlConnection.Open();
        }
        return _sqlConnection;
    }

    public void Dispose()
    {
        if (_sqlConnection != null)
        {
            _sqlConnection.Close();
            _sqlConnection.Dispose();
        }
    }
}
