using System;

namespace RC.Interfaces.Storages
{
    public interface IStorageSetup
    {
        bool Active { get; set; }
        string Description { get; set; }
        string Name { get; set; }
        Uri Uri { get; set; }
    }
}