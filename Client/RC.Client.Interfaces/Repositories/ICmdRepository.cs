using RC.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC.Client.Interfaces.Repositories
{
    public interface ICmdRepository
    {
        CmdResult<TResult, TCmdParamsSet> RetrieveResult<TResult, TCmdParamsSet>(CmdParametersSet parametersSet) 
            where TCmdParamsSet : CmdParametersSet;
        void Save<TCmdParams>(TCmdParams cmdParams);
        void SetAsConsumed(CmdParametersSet cmdParams);

    }

    public interface ICmdRepositoryAsync
    {

        Task<string> RetrieveExecutedCmdResultJsonAsync<TCmdParamsSet>(TCmdParamsSet parametersSet)
          where TCmdParamsSet : CmdParametersSet;
        Task SendToBackendAsync<TCmdParams>(TCmdParams cmdParams) where TCmdParams : CmdParametersSet;

        Task<TCmdParamsSet> RetrieveExecutedCmdAsync<TCmdParamsSet>(TCmdParamsSet parametersSet)
           where TCmdParamsSet : CmdParametersSet;

        Task<CmdResult<TResult, TCmdParamsSet>> RetrieveResultAsync<TResult, TCmdParamsSet>(TCmdParamsSet parametersSet,TimeSpan timeout)
         where TCmdParamsSet : CmdParametersSet;
    }
}
