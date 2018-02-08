using RC.Interfaces.Appenders;
using RC.Interfaces.Storages;
using System.Collections.Generic;

namespace RC.Implementation.Commands
{
    public class FileSystemContentsListingCmd : AbstractCmd<IEnumerable<IStorageObject>>
    {
        protected readonly IStorage<IStorageObject> _storage;

        public FileSystemContentsListingCmd(IStorage<IStorageObject> storage, IResultAppender resultAppender): base(resultAppender)
        {
            _storage = storage;
        }

        protected override IEnumerable<IStorageObject> RunCommand()
        {
            return _storage.Contents();
        }
    }
}
