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
        private IDictionary<CmdType, Func<IDictionary<string, string>, ICmd>> _map = new Dictionary<CmdType, Func<IDictionary<string, string>, ICmd>>();

        public  CmdFactory()
        {
            BuildMap();
        }
        public ICmd Create(CmdType cmdType, IDictionary<string, string> cmdParams)
        {
            return _map[cmdType](cmdParams);


        }

        private void BuildMap()
        {
            var resultAppender = ResultAppenderManager.Instance.ResultAppender;

           // _map.Add(CmdType.StorageContentsListing, (Dictionary<string, string> cmdParams) => { return new FileSystemContentsListingCmd(new StorageFactory(), resultAppender, cmdParams); });
        }
    }

    
}