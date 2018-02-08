using RC.Implementation.Storages;
using RC.Interfaces.Factories;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.Infrastructure.Factories
{
    public static class StorageFactory
    {
        private static readonly IDictionary<StorageType, Func<Uri, IStorage<IStorageObject>>> _map;

        static StorageFactory()
        {
            _map = BuildMap();
        }
        public static IStorage<IStorageObject> Create(StorageType type, Uri storageUri)
        {
            return _map[type](storageUri);
        }

        private static IDictionary<StorageType, Func<Uri,IStorage<IStorageObject>>> BuildMap()
        {
            return new Dictionary<StorageType, Func<Uri, IStorage<IStorageObject>>>
            {
                {StorageType.FileSystem, (Uri uri) => new LocalFileSystemStorage(new BasicStorageSetup(uri)) }
            };
        }
    }
}
