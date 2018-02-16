using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.Infrastructure.Factories
{
    public class DBConnectionFactory :IDbConnectionFactory
    {
        private readonly string _providerName;
        private readonly string _connectionString;

        //from https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/obtaining-a-dbproviderfactory
        // Given a provider name and connection string, 
        // create the DbProviderFactory and DbConnection.
        // Returns a DbConnection on success; null on failure.

        public DBConnectionFactory(string providerName, string connectionString)
        {
            _providerName = providerName;
            _connectionString = connectionString;
        }

        public DBConnectionFactory(ConnectionStringSettings settings)
        {
            _providerName = settings.ProviderName;
            _connectionString = settings.ConnectionString;
        }

        public IDbConnection CreateDbConnection()
        {
            // Assume failure.
            DbConnection connection = null;

            // Create the DbProviderFactory and DbConnection.
            if (_connectionString != null)
            {
                try
                {
                    DbProviderFactory factory =
                        DbProviderFactories.GetFactory(_providerName);

                    connection = factory.CreateConnection();
                    connection.ConnectionString = _connectionString;
                }
                catch (Exception ex)
                {
                    // Set the connection to null if it was created.
                    if (connection != null)
                    {
                        connection = null;
                    }
                    throw ex;
                }
            }
            // Return the connection.
            return connection;
        }

    }
}
