using RC.Implementation.Commands;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    public class CmdFactory : ICmdFactory
    {
        private readonly IDictionary<CmdType, Func<Uri, ICmd>> _Map;

        public CmdFactory()
        {
            _Map = BuildMap();
        }
        public ICmd Create(CmdType cmdType, IDictionary<string, string> cmdParams)
        {
            var uri = new Uri(cmdParams["uri"]);
            return _Map[cmdType](uri);


        }

        protected IDictionary<CmdType, Func<Uri, ICmd>> BuildMap()
        {
            var resultAppender = ResultAppenderManager.Instance.ResultAppender;

            return new Dictionary<CmdType, Func<Uri, ICmd>>
                {
                    { CmdType.StorageContentsListing, (Uri uri)=> { return new FileSystemContentsListingCmd(StorageFactory.Create(StorageType.FileSystem,uri),resultAppender); } }
                };
        }
    }

    
}