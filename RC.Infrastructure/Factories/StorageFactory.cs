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
    public class StorageFactory : IStorageFactory
    {
        private readonly IDictionary<StorageType, Func<Uri, IStorage<IStorageObject>>> _map;

        public StorageFactory()
        {
            _map = BuildMap();
        }
        public IStorage<IStorageObject> Create(StorageType type, Uri storageUri)
        {
            return _map[type](storageUri);
        }

        private IDictionary<StorageType, Func<Uri,IStorage<IStorageObject>>> BuildMap()
        {
            return new Dictionary<StorageType, Func<Uri, IStorage<IStorageObject>>>
            {
                {StorageType.FileSystem, (Uri uri) => new LocalFileSystemStorage(new BasicStorageSetup(uri)) }
            };
        }
    }
}
