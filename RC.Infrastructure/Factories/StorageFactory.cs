using RC.Domain.Storages;
using RC.Implementation.Storages;
using RC.Interfaces.Factories;
using RC.Interfaces.Infrastructure;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    //this singleton follows fourth Jon Skeet's version http://csharpindepth.com/Articles/General/Singleton.aspx 

    public sealed class StorageFactory : IStorageFactory
    {
        private readonly IDictionary<StorageType, Func<Uri, IStorage<IStorageObject>>> _map;
        private readonly IStorageSettings _storageSettings;

        public StorageFactory(IStorageSettings storageSettings)
        {
            _storageSettings = storageSettings;
            _map = BuildMap();
        }
        
        public IStorage<IStorageObject> Create(StorageType type, Uri storageUri)
        {
            return _map[type](storageUri);
        }

        private IDictionary<StorageType, Func<Uri, IStorage<IStorageObject>>> BuildMap()
        {
            return new Dictionary<StorageType, Func<Uri, IStorage<IStorageObject>>>
            {
                {StorageType.FileSystem, (Uri uri) => new LocalFileSystemStorage(_storageSettings.GetSetup(uri)) }
            };
        }
    }
}
