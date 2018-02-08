using System;
using System.Collections.Generic;
using System.IO;
using RC.Interfaces.Storages;
using System.Linq;

namespace RC.Implementation.Storages
{
    public class LocalFileSystemStorage : AbstractStorage<SimpleStorageObject>
    {

        public LocalFileSystemStorage(IStorageSetup setup): base(setup)
        {
        }

        public override IEnumerable<SimpleStorageObject> Contents()
        {
            var entries = Directory.EnumerateFileSystemEntries(_setup.Uri.OriginalString, "*", SearchOption.AllDirectories);
            return entries.Select(path => new SimpleStorageObject(new Uri(path)));
        }
    }
}