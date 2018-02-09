using System;
using RC.Interfaces.Storages;

namespace RC.Interfaces.Factories
{
    public interface IStorageFactory
    {
        IStorage<IStorageObject> Create(StorageType type, Uri storageUri);
    }
}