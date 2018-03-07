using RC.Domain.Commands;
using System.Threading.Tasks;

namespace RC.Client.Interfaces
{
    public interface IRemoteCmdCaller
    {
        Task<CmdResult<TResult, TCmdParamsSet>> CallAsync<TResult, TCmdParamsSet>(TCmdParamsSet cmdParams) where TCmdParamsSet : CmdParametersSet;
    }
}
