using System.Data;
using System.Data.Common;

namespace RC.Interfaces.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
    }
}