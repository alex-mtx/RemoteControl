using RC.Client.Interfaces;
using RC.Domain.Commands.Storages;
using RC.Domain.Storages;
using System;
using System.Collections.Generic;

namespace RC.Client.Commands
{
    public class FileSystemContentsListingDispatcher : AbstractCmdDispatcher<List<SimpleStorageObject>, StorageCmdParamSet>
    {
        private Uri _targetDirectory;

        public FileSystemContentsListingDispatcher(Uri targetDirectory, IRemoteCmdCaller remoteCmdCaller) : base(remoteCmdCaller)
        {
            if (!targetDirectory.IsAbsoluteUri)
                throw new ArgumentException(targetDirectory.ToString() + " must be absolute",nameof(targetDirectory));
            _targetDirectory = targetDirectory;
        }

        protected override StorageCmdParamSet ParamsCompletion()
        {
            return new StorageCmdParamSet { Path = _targetDirectory.OriginalString };
        }
    }
}
