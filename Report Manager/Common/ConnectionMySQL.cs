using MySql.Data.MySqlClient;

namespace Report_Manager.Common
{

    internal class ConnectionMySQL
    {
        

        

        readonly MySqlConnection connection = new();

        public ConnectionMySQL()
        {
            connection.ConnectionString = @"Server = " + Globals.server + "; Database=report_manager;Uid=newuser;Pwd=New@Mic15;";
        }

        public MySqlConnection Connect()
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
}
