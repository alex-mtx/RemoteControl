using RC.Client.Interfaces;
using RC.Client.Interfaces.Repositories;
using RC.Domain.Commands;
using System;
using System.Threading.Tasks;

namespace RC.Client
{
    public class RemoteCommandCaller : IRemoteCmdCaller
    {
        private readonly ICmdRepositoryAsync _repo;

        public RemoteCommandCaller(ICmdRepositoryAsync repo)
        {
            _repo = repo;
        }

        public virtual async Task<CmdResult<TResult, TCmdParamsSet>> CallAsync<TResult, TCmdParamsSet>(TCmdParamsSet cmdParams)
            where TCmdParamsSet : CmdParametersSet
        {
            await _repo.SendToBackendAsync(cmdParams);
            var cmdParamsExecuted = await _repo.RetrieveExecutedCmdAsync(cmdParams);
        }
    }
}
