using RC.Interfaces.Storages;
using System;

namespace RC.Implementation.Storages
{
    public class BasicStorageSetup : IStorageSetup
    {
        public Uri Uri { get; private set; }
        public BasicStorageSetup(Uri uri) {

            if (!uri.IsAbsoluteUri)
                throw new ArgumentException("An absolute Uri is required");
            Uri = uri;
        }
    }
}
