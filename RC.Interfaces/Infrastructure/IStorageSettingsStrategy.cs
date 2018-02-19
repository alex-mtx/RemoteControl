using System;
using RC.Interfaces.Storages;

namespace RC.Interfaces.Setup
{
    public interface IStorageSettingsStrategy
    {
        IStorageSetup GetSetup(Uri uri);
    }
}