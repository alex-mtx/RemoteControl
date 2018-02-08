using System;
using System.Collections.Generic;
using RC.Interfaces.Storages;

namespace RC.Implementation.Storages
{
    public class SimpleStorageObject : IStorageObject
    {
        public Uri Uri { get; private set; }

        public IDictionary<StorageObjectAttribute, string> Attributes { get; private set; }

        public SimpleStorageObject(Uri uri) => Uri = uri;
    }
}