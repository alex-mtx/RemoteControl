using System;
using RC.Interfaces.Setup;
using RC.Interfaces.Storages;

namespace RC.JsonServices.Setup
{
    public class JsonStorageSettingsStrategy : IStorageSettingsStrategy
    {
        public IStorageSetup GetSetup(Uri uri)
        {
            throw new NotImplementedException();
        }
    }
}