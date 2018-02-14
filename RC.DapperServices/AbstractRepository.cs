using RC.Interfaces.Factories;
using System;
using System.Data;

namespace RC.DapperServices
{
    public abstract class AbstractRepository
    {
        protected readonly string _connectionString;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AbstractRepository(IDbConnectionFactory dbConnectionFactory) => _dbConnectionFactory = dbConnectionFactory;

        protected virtual void Execute(Action<IDbConnection> query)
        {
            using (IDbConnection conn = _dbConnectionFactory.CreateDbConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        query(conn);
                        tx.Commit();
                    }
                    catch (Exception e)
                    {
                        if (tx != null)
                            tx.Rollback();
                        throw e;
                    }
                }
            }
        }

        protected virtual TReturn Query<TReturn>(Func<IDbConnection, TReturn> query)
        {
            using (IDbConnection conn = _dbConnectionFactory.CreateDbConnection())
            {
                return query(conn);
            }
        }
    }
}
