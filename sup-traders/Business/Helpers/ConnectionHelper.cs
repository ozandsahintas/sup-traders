using Microsoft.Data.SqlClient;
using System.Data;

namespace sup_traders.Business.Helpers
{
    public class ConnectionHelper
    {
        private readonly string? _connectionString;

        public ConnectionHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public IDbConnection CreateSqlConnection() => new SqlConnection(_connectionString);
    }
}
