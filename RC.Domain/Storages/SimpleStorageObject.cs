using System;
using System.Collections.Generic;

namespace RC.Domain.Storages
{
    public class SimpleStorageObject 
    {
        public Uri Uri { get; private set; }

        public IDictionary<StorageObjectAttribute, string> Attributes { get; private set; }

        public SimpleStorageObject(Uri uri) => Uri = uri;
    }
}