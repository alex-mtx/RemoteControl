using System;

namespace RC.Implementation.Commands.Storages
{
    public class StorageCmdParamSet : CmdParametersSet
    {
        protected Uri Uri { get; set; }
        public string Path { get => Uri.LocalPath; set => Uri = new Uri(value); }
    }
}
