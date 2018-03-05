using RC.Domain.Commands;
using System;

namespace RC.Domain.Commands.Storages
{
    public class StorageCmdParamSet : CmdParametersSet
    {
        public StorageCmdParamSet()
        {

        }
        protected Uri Uri { get; set; }
        public string Path { get => Uri.LocalPath; set => Uri = new Uri(value,UriKind.Absolute); }
    }
}
