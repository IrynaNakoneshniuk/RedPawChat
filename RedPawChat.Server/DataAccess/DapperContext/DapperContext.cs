using System.Data;
using System.Data.SqlClient;

namespace RedPawChat.Server.DataAccess.DapperContext
{
    // DapperContext is a class responsible for managing database connections using Dapper, a micro ORM for .NET.
    public class DapperContext:IDapperContext
    {
        // IConfiguration is used to access configuration settings, such as connection strings.
        private readonly IConfiguration _iConfiguration;

        // _connString stores the connection string retrieved from the configuration.
        private readonly string _connString;

        // Constructor that takes an IConfiguration instance to retrieve connection string from configuration.
        public DapperContext(IConfiguration iConfiguration)
        {
            _iConfiguration = iConfiguration;

            // Retrieve the connection string named "connMSSQL" from the configuration.
            _connString = _iConfiguration.GetConnectionString("DefaultConnection");
        }

        // CreateConnection method returns a new IDbConnection instance (SqlConnection in this case) using the stored connection string.
        // This method allows users of the class to easily create a new database connection.
        public IDbConnection CreateConnection() => new SqlConnection(_connString);
    }

}
