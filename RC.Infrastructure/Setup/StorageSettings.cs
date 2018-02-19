using RC.Implementation.Storages;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System;
using RC.JsonServices.Setup;
using RC.Interfaces.Setup;

namespace RC.Infrastructure.Setup
{
    public class StorageSettings : IStorageSettings
    {
        public StorageSettings()
        {
            Strategy = JsonStorageSettingsStrategy.Instance;
        }
        public IStorageSettingsStrategy Strategy { get; set; }
        public IStorageSetup GetSetup(Uri uri)
        {
            return Strategy.GetSetup(uri);
        }
    }
}
