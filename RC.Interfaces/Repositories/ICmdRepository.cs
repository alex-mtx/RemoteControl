using System.Collections.Generic;
using System.Data;

namespace RC.Interfaces.Repositories
{
    public interface ICmdRepository<out T>
    {
        IEnumerable<T> PendingCommands();
    }
}