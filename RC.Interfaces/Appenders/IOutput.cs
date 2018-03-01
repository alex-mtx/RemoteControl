using RC.Domain.Commands;
using System;

namespace RC.Interfaces.Appenders
{
    public interface IOutput<in TCmdParamsSet> where TCmdParamsSet : CmdParametersSet
    {
        void Send<TResult>(CmdResult<TResult, CmdParametersSet> cmdResult);
    }
}
