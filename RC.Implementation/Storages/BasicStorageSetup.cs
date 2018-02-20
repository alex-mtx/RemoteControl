using RC.Interfaces.Storages;
using System;

namespace RC.Implementation.Storages
{
    public class BasicStorageSetup : IStorageSetup
    {
        private string _description;

        public Uri Uri { get; }

        public string Description { get => DefaultIfEmptyDescription(); set => _description = value; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public BasicStorageSetup(string uri, string name, bool active, string description = null)
        {
            Uri = ValidatedUri(uri);
            Name = name;
            Active = active;
            Description = description;
        }

       

        private Uri ValidatedUri(string path)
        {
            Uri uri;
            try
            {
                uri = new Uri(path, UriKind.Absolute);
                if(!uri.IsAbsoluteUri)
                    throw new ArgumentException($"The provided path '{path}' is not an absolute Uri");
            }
            catch (UriFormatException ex)
            {
                throw new ArgumentException($"The provided path '{path}' is not an absolute Uri");
            }
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
