﻿using RC.Domain.Storages;
using RC.Implementation.Storages;
using RC.Interfaces.Factories;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    //this singleton follows fourth Jon Skeet's version http://csharpindepth.com/Articles/General/Singleton.aspx 

    public sealed class StorageFactory : IStorageFactory
    {
        private readonly IDictionary<StorageType, Func<Uri, IStorage<IStorageObject>>> _map;
        private static StorageFactory _instance = new StorageFactory();

        static StorageFactory()
        {

        }

        private StorageFactory()
        {
            _map = BuildMap();
        }
        public static StorageFactory Instance
        {
            get
            {
                return _instance;
            }
        }


       
        public IStorage<IStorageObject> Create(StorageType type, Uri storageUri)
        {
            return _map[type](storageUri);
        }

        private IDictionary<StorageType, Func<Uri, IStorage<IStorageObject>>> BuildMap()
        {
            return new Dictionary<StorageType, Func<Uri, IStorage<IStorageObject>>>
            {
                {StorageType.FileSystem, (Uri uri) => new LocalFileSystemStorage(new BasicStorageSetup(uri,"fake repo",true)) }
            };
        }
    }
}
