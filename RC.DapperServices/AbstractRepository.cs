using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.DapperServices
{
    public class AbstractRepository
    {
        private DbConnection _connection;

        public AbstractRepository(DbConnection connection)
        {
            _connection = connection;
        }
    }
}
