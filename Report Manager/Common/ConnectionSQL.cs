using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Manager.Common;

internal class ConnectionSQL
{
    readonly SqlConnection connection = new();

    public ConnectionSQL()
    {
        connection.ConnectionString = @"Server=tcp:reportmanager.database.windows.net,1433;Initial Catalog=report-manager;Persist Security Info=False;User ID=reportmanager;Password=Horizon@Crusader;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }

    public SqlConnection Connect()
    {
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            connection.Open();
        }

        return connection;
    }

    public void Disconnect()
    {
        if (connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
        }
    }
}
