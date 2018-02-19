using RC.Implementation.Storages;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System;

namespace RC.Infrastructure.Setup
{
    public class StorageSettings : IStorageSettings
    {
        public IStorageSetup GetSetup(Uri uri)
        {
            return new BasicStorageSetup(uri, "a name", true);
        }
    }
}
