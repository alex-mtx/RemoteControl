using System;

namespace RC.Interfaces.Storages
{
    [Flags]
    public enum StorageObjectAttribute
    {
        Default = 0,
        Length = 1,
        CreationDate = 2,
        LastModificationDate = 3
    }
}