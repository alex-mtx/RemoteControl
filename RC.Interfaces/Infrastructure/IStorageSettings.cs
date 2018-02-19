using System;
using RC.Interfaces.Storages;

namespace RC.Interfaces.Infrastructure
{
    public interface IStorageSettings
    {
        IStorageSetup GetSetup(Uri uri);
    }
}