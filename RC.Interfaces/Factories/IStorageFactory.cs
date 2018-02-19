using System;
using RC.Domain.Storages;
using RC.Interfaces.Storages;

namespace RC.Interfaces.Factories
{
    public interface IStorageFactory
    {
        IStorage<IStorageObject> Create(StorageType type, Uri storageUri);
    }
}