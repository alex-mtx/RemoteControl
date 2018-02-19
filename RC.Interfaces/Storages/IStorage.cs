using System;
using System.Collections.Generic;

namespace RC.Interfaces.Storages
{
    public interface IStorage<out TStorageObject>
    {
        IEnumerable<TStorageObject> Contents();
        IStorageSetup Info();
    }
}