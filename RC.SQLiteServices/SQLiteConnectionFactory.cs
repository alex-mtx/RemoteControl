using RC.Interfaces.Factories;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace RC.SQLiteServices
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        private readonly string _providerName;
        private readonly string _connectionString;

        //from https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/obtaining-a-dbproviderfactory
        // Given a provider name and connection string, 
        // create the DbProviderFactory and DbConnection.
        // Returns a DbConnection on success; null on failure.

        public SQLiteConnectionFactory(string providerName, string connectionString):this(connectionString)
        {
            _providerName = providerName;
        }
        /// <summary>
        /// Assumes "System.Data.SQLite" as the default provider
        /// </summary>
        /// <param name="connectionString"></param>
        public SQLiteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
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
                    connection = new SQLiteConnection(_connectionString);
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
