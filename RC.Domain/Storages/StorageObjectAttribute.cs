using System;

namespace RC.Domain.Storages
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