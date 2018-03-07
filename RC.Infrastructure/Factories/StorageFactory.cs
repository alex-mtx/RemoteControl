using RC.Domain.Storages;
using RC.Implementation.Storages;
using RC.Interfaces.Factories;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    public sealed class StorageFactory : IStorageFactory
    {
        private readonly IDictionary<StorageType, Func<Uri, IStorage<SimpleStorageObject>>> _map;
        private readonly IStorageSettings _storageSettings;

        public StorageFactory(IStorageSettings storageSettings)
        {
            _storageSettings = storageSettings;
            _map = BuildMap();
        }
        
        public IStorage<SimpleStorageObject> Create(StorageType type, Uri storageUri)
        {
            return _map[type](storageUri);
        }

        private IDictionary<StorageType, Func<Uri, IStorage<SimpleStorageObject>>> BuildMap()
        {
            return new Dictionary<StorageType, Func<Uri, IStorage<SimpleStorageObject>>>
            {
                {StorageType.FileSystem, (Uri uri) => new LocalFileSystemStorage(_storageSettings.GetSetup(uri)) }
            };
        }
    }
}
