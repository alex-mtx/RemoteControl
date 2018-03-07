using RC.Interfaces.Factories;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RC.Data
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
                    catch (Exception)
                    {
                        if (tx != null)
                            tx.Rollback();
                        throw;
                    }
                }
            }
        }

        protected virtual void Execute(Action<IDbConnection,IDbTransaction> query)
        {
            using (IDbConnection conn = _dbConnectionFactory.CreateDbConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        query(conn,tx);
                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        if (tx != null)
                            tx.Rollback();
                        throw;
                    }
                }
            }
        }

        protected virtual async Task ExecuteAsync(Func<IDbConnection, IDbTransaction,Task> query)
        {
            using (IDbConnection conn = _dbConnectionFactory.CreateDbConnection())
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        await query(conn, tx);
                        tx.Commit();
                    }
                    catch (Exception)
                    {
                        if (tx != null)
                            tx.Rollback();
                        throw;
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
