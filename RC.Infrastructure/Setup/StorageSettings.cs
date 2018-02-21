using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System;

namespace RC.Infrastructure.Setup
{
    public class StorageSettings : IStorageSettings
    {

        public StorageSettings(IStorageSettingsStrategy strategy )
        {
            Strategy = strategy;
        }
        public IStorageSettingsStrategy Strategy { get; set; }
        public IStorageSetup GetSetup(Uri uri)
        {
            return Strategy.GetSetup(uri);
        }
    }
}
