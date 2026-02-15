using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Organization_API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult result;
        protected string body = "";
        private static SqlCommand sqlCommand = new SqlCommand();
        //oridinarily connection strings would be stored in a secure configuration file or environment variable
        //but this is demonstration of the API functionality
        private const string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=E:\\repos\\Organization_API\\Organization_API\\Data\\Organization.mdf;Integrated Security=True";

        protected static DataTable GetData(string SQL)
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
