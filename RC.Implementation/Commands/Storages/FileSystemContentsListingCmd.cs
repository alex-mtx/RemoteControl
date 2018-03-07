using RC.Domain.Commands.Storages;
using RC.Domain.Storages;
using RC.Interfaces.Appenders;
using RC.Interfaces.Factories;
using RC.Interfaces.Storages;
using System;
using System.Collections.Generic;

namespace RC.Implementation.Commands.Storages
{
    public class FileSystemContentsListingCmd: AbstractCmd<StorageCmdParamSet,IEnumerable<SimpleStorageObject>>
    {
        protected readonly IStorage<SimpleStorageObject> _storage;
        public FileSystemContentsListingCmd(IStorageFactory storageFactory , IResultAppender<StorageCmdParamSet> resultAppender, StorageCmdParamSet args) : base(resultAppender,args)
        {
            _storage = storageFactory.Create(StorageType.FileSystem,new Uri(args.Path));
        }

        protected override IEnumerable<SimpleStorageObject> RunCommand()
        {
            return _storage.Contents();
        }
    }
}
