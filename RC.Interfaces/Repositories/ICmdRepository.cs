﻿using System.Collections.Generic;
using System.Data;

namespace RC.Interfaces.Repositories
{
    public interface ICmdRepository<T>
    {
        IEnumerable<T> PendingCommands();
        void Update(T entity);
    }
}