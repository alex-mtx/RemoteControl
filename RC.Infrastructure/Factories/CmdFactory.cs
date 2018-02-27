using RC.Domain.Commands;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Appenders;
using RC.Interfaces.Commands;
using RC.Interfaces.Factories;
using System;
using System.Collections.Generic;

namespace RC.Infrastructure.Factories
{
    public class CmdFactory : ICmdFactory<CmdParametersSet>
    {
        private IDictionary<CmdType, Func<CmdParametersSet, ICmd>> _map = new Dictionary<CmdType, Func<CmdParametersSet, ICmd>>();

        public  CmdFactory(IResultAppender<CmdParametersSet> resultAppender, IStorageFactory storageFactory)
        {
            BuildMap(resultAppender, storageFactory);
            
        }
        public ICmd Create(CmdType cmdType, CmdParametersSet parametersSet)
        {
            return _map[cmdType](parametersSet);
        }


        private void BuildMap(IResultAppender<CmdParametersSet> resultAppender, IStorageFactory storageFactory)
        {

            _map.Add(
                        CmdType.StorageContentsListing, 
                        (CmdParametersSet parametersSet) => 
                            {
                                return new FileSystemContentsListingCmd(storageFactory, resultAppender, Convert<StorageCmdParamSet>(parametersSet));
                            }
                    );
        }

        private  TNewType Convert<TNewType>(CmdParametersSet o)
        {
            return (TNewType)System.Convert.ChangeType(o, typeof(TNewType));
        }
    }

    
}