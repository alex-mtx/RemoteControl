using RC.Client.Interfaces;
using RC.Domain.Commands;
using System;
using System.Threading.Tasks;

namespace RC.Client.Commands
{
    public abstract class AbstractCmdDispatcher<TResult, TCmdParamsSet>
             where TCmdParamsSet : CmdParametersSet
             where TResult : new()
    {
        private readonly IRemoteCmdCaller _remoteCmdCaller;

        public AbstractCmdDispatcher(IRemoteCmdCaller remoteCmdCaller)
        {
            _remoteCmdCaller = remoteCmdCaller;
        }
        public async Task<CmdResult<TResult, TCmdParamsSet>> SubmitAsync(string perUserSessionId)
        {
            if (string.IsNullOrEmpty(perUserSessionId))
                throw new ArgumentException("cannot be null or empty", nameof(perUserSessionId));

            TCmdParamsSet cmdParamsSet = ParamsCompletion();
            cmdParamsSet.Issuer = perUserSessionId;
            cmdParamsSet.RequestId = Guid.NewGuid();
            cmdParamsSet.SentOn = DateTime.Now;

            CmdResult<TResult, TCmdParamsSet> result = default(CmdResult<TResult, TCmdParamsSet>);

            result = await _remoteCmdCaller.CallAsync<TResult, TCmdParamsSet>(cmdParamsSet);
            return result;

        }

        protected abstract TCmdParamsSet ParamsCompletion();


    }
}
