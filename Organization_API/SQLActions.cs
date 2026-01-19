using Microsoft.Data.SqlClient;
using System.Data;

namespace Organization_API
{
    /// <summary>
    /// A base controller class that manages SQL database connections and other general functionalities.
    /// </summary>
    public static class SQLActions
    {
        private static SqlCommand sqlCommand = new SqlCommand();
        private const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Organization;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public static DataTable GetData(string SQL)
        {
            DataTable dataTable = new DataTable();
            sqlCommand.CommandText = SQL;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.Connection = new SqlConnection(connectionString);
            sqlCommand.Connection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            dataTable.Load(reader);
            sqlCommand.Connection.Close();

            return dataTable;
        }
    }
}
