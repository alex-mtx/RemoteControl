using System;
using RC.Interfaces.Storages;

namespace RC.Interfaces.Infrastructure
{
    public interface IStorageSettingsStrategy
    {
        IStorageSetup GetSetup(Uri uri);
    }
}