using RC.Interfaces.Storages;
using System;

namespace RC.Implementation.Storages
{
    public class BasicStorageSetup : IStorageSetup
    {
        private Uri _uri;
        private string _description;

        public Uri Uri { get => _uri; set => _uri = ValidatedUri(value); }

        public string Description { get => DefaultIfEmptyDescription(); set => _description = value; }

      
        public string Name { get; set; }
        public bool Active { get; set; }

        public BasicStorageSetup(Uri uri, string name, bool active, string description = null)
        {
            Uri = ValidatedUri(uri);
            Name = name;
            Active = active;
            Description = description;
        }

        public BasicStorageSetup(string path, string name, bool active, string description = null) : this (new Uri(path),name,active,description)
        {

        }

        private Uri ValidatedUri(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                throw new ArgumentException($"The provided path '{uri.OriginalString}' is not an absolute Uri");
            return uri;
        }

        private string DefaultIfEmptyDescription()
        {
            if (string.IsNullOrEmpty(_description))
                return "not provided";

            return _description;
        }

    }
}
