using System.Data.SqlClient;

namespace Play3_Client_Side.API
{
    class SqlProvider
    {
        private string connectionString;
        private SqlConnection connection;
        private SqlDataAdapter adapter;

        public SqlProvider()
        {
            connectionString = "";
            connection = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter();
        }

        public void Insert(string sql)
        {
            SqlCommand command = new SqlCommand(sql, connection);
            adapter.InsertCommand = new SqlCommand(sql, connection);
            adapter.InsertCommand.ExecuteNonQuery();

            command.Dispose();
            connection.Close();
        }

        public void Delete(string sql)
        {
            SqlCommand command = new SqlCommand(sql, connection);
            adapter.DeleteCommand = new SqlCommand(sql, connection);
            adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            connection.Close();
        }
    }
}
