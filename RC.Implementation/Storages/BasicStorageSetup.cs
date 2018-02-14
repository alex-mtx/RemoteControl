using RC.Interfaces.Storages;
using System;

namespace RC.Implementation.Storages
{
    public class BasicStorageSetup : IStorageSetup
    {
        public Uri Uri { get; private set; }
        public BasicStorageSetup(Uri uri) {

            if (!uri.IsAbsoluteUri)
                throw new ArgumentException($"The provided path '{uri.OriginalString}' is not an absolute Uri");
            Uri = uri;
        }
    }
}
