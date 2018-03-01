using RC.Domain.Commands;
using System.Collections.Generic;

namespace RC.Interfaces.Repositories
{
    public interface ICmdRepository<out TReturnCmdParams,in TCmdParams>
        where TReturnCmdParams : CmdParametersSet
    {
        IEnumerable<TReturnCmdParams> PendingCommands();
        void Update<TCmdReturn>(CmdResult<TCmdReturn, CmdParametersSet> cmdResult);
        void Update(CmdParametersSet cmdParameters);

    }
}