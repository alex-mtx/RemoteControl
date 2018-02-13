using RC.Implementation.Commands;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    public class CmdFactory : ICmdFactory<CmdParametersSet>
    {
        private IDictionary<CmdType, Func<CmdParametersSet, ICmd>> _map = new Dictionary<CmdType, Func<CmdParametersSet, ICmd>>();

        public  CmdFactory()
        {
            BuildMap();
        }
        public ICmd Create(CmdType cmdType, CmdParametersSet parametersSet)
        {
            return _map[cmdType](parametersSet);


        }


        private void BuildMap()
        {
            var resultAppender = ResultAppenderManager.Instance.ResultAppender;

            _map.Add(
                        CmdType.StorageContentsListing, 
                        (CmdParametersSet parametersSet) => 
                            {
                                return new FileSystemContentsListingCmd(StorageFactory.Instance, resultAppender, Convert<StorageCmdParamSet>(parametersSet));
                            }
                    );
        }

        private  T Convert<T>(object o) where T : CmdParametersSet
        {
            return (T)System.Convert.ChangeType(o, typeof(T));
        }
    }

    
}