using System;
using System.Collections.Generic;

namespace RC.Interfaces.Storages
{
    public interface IStorageObject
    {
        Uri Uri { get; }
        IDictionary<StorageObjectAttribute, string> Attributes { get; }
    }
}